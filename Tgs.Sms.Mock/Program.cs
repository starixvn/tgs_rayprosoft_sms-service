using FluentdForward.OpenTelemetry.MessagePack;
using NATS.Client.Core;
using NATS.Client.JetStream.Models;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Telegram.Bot;
using Tgs.MessageQueue.Nats.Serializers;
using Tgs.Sms.Mock.MessageQueue;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Logging
        .AddOpenTelemetry(options =>
        {
            options.IncludeFormattedMessage = true;
            options.ParseStateValues = true;
            _ = options.AddFluentdForwardExporter(fluentOptions =>
            {
                var nodeIp = builder.Configuration.GetValue<string>("NODE_IP")!;
                fluentOptions.Host = nodeIp;
                fluentOptions.Tag = builder.Configuration.GetValue<string>("OTEL_SERVICE_NAME")!;
                fluentOptions.UseMessagePack(LogRecordFormatterResolver.GetResolverInstanceWithExtendInfo(
                    "MachineInfo",
                    () => new
                    {
                        Environment.MachineName
                    }));
            });
        });

    builder.Services
        .AddKeyedSingleton(
            "Nats",
            (sp, _) => new NatsConnection(NatsOpts.Default with
            {
                Url = builder.Configuration.GetConnectionString("Nats")!,
                SerializerRegistry = TgsNatsSerializerRegistration.Default
            }))
        .AddHttpClient<ITelegramBotClient, TelegramBotClient>((httpClient) => new TelegramBotClient(
            builder.Configuration.GetValue<string>("Mock:Token")!,
            httpClient))
        .Services
        .AddMessageQueue()
        .AddNatsMessageQueue(config => config
            .ConfigureResolveConnection(sp => sp.GetRequiredKeyedService<NatsConnection>("Nats"))
            .ConfigJetStream(new StreamConfig("tgs_sms_mock", ["tgs.sms.mock"]) with
            {
                Storage = StreamConfigStorage.File,
                Retention = StreamConfigRetention.Workqueue,
                NumReplicas = builder.Configuration.GetValue<int>("Mock:JetStreamReplicas")
            })
            .AddJetStreamHandler(
                "tgs.sms.mock",
                "tgs_sms_mock",
                new ConsumerConfig("mock_mail_slot") with
                {
                    MaxAckPending = 1
                },
                handlerFactory: sp => ActivatorUtilities.CreateInstance<SmsMessageReceiveHandler>(
                    sp,
                    builder.Configuration.GetValue<long>("Mock:ChatId"))));

    var excludeExtensions = new[] { "html", "ico", "png", "jpg" };
    builder.Services
        .AddOpenTelemetry()
        .WithTracing(tracingBuilder => tracingBuilder
            .SetResourceBuilder(ResourceBuilder.CreateDefault()
                .AddEnvironmentVariableDetector())
            .AddSource("Tgs.*")
            .AddSource("TGS.*")
            .AddSource("Valhalla.*")
            .AddAspNetCoreInstrumentation(options => options.Filter =
            httpContext => !httpContext.Request.Path.HasValue
                || !httpContext.Request.Path.StartsWithSegments("/swagger")
                && !httpContext.Request.Path.StartsWithSegments("/healthz")
                && !httpContext.Request.Path.StartsWithSegments("/metrics")
                && !httpContext.Request.Path.StartsWithSegments("/profiler/")
                && !excludeExtensions.Any(
                    ext => httpContext.Request.Path.Value.EndsWith($".{ext}")))
            .AddProfileViewExporter()
            .AddOtlpExporter())
        .WithMetrics(meterBuiler => meterBuiler
            .SetResourceBuilder(ResourceBuilder.CreateDefault()
                .AddEnvironmentVariableDetector())
                .AddRuntimeInstrumentation()
                .AddAspNetCoreInstrumentation()
                .AddOtlpExporter())
        .Services
        .AddProfileViewer()
        .AddHealthChecks();
}

var app = builder.Build();
{
    app.MapHealthChecks("/healthz");
    app.MapProfileViewer();
}

app.Run();
