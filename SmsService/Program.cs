using System.Net;
using FluentdForward.OpenTelemetry.MessagePack;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using TGS.SmsService;
using TGS.SmsService.Metrics;

var builder = WebApplication.CreateBuilder(args);
SmsApiMeter.Init();

builder.Logging
    .AddOpenTelemetry(options =>
    {
        options.IncludeFormattedMessage = true;
        options.ParseStateValues = true;
        _ = options.AddFluentdForwardExporter(fluentOptions =>
        {
            var nodeIp = builder.Configuration.GetValue<string>("NODE_IP");
            fluentOptions.Host = nodeIp!;
            fluentOptions.Tag = builder.Configuration.GetValue<string>("OTEL_SERVICE_NAME")!;
            fluentOptions.UseMessagePack(LogRecordFormatterResolver.GetResolverInstanceWithExtendInfo(
                "MachineName",
                () => Environment.MachineName));
        });
    });

builder.Services
    .Configure<ForwardedHeadersOptions>(options =>
    {
        options.KnownNetworks.Add(
            new Microsoft.AspNetCore.HttpOverrides.IPNetwork(IPAddress.Parse("172.0.0.0"), 8));
        options.KnownNetworks.Add(
            new Microsoft.AspNetCore.HttpOverrides.IPNetwork(IPAddress.Parse("10.0.0.0"), 8));
        options.ForwardedHeaders = ForwardedHeaders.All;
    });

builder.Services
    .AddControllers()
    .AddMvcOptions(options =>
    {
        options.Filters.Add<ControllerDiagnosticsFilter>();
    });

builder.Services
    .AddSwaggerGen(options =>
    {
        options.SwaggerDoc(
            "v1",
            new OpenApiInfo
            {
                Title = "SmsService API",
                Version = "v1"
            });

        options.ApplyWebApiToolkitSupport();
    });

builder.Services
    .AddSmsServiceCore()
    .AddSmsServiceRepositories((sp, dbContextBuilder) =>
    {
        var config = sp.GetRequiredService<IConfiguration>();

        dbContextBuilder.UseNpgsql(config.GetConnectionString("Tgs")!);
    })
    .AddPhoenixToolkitsMessageQueue()
    .AddRabbitMessageQueue(config => config
        .ConfigQueueOptions((options, sp) =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();

            configuration.GetSection("Sms").Bind(options);
        }))
    .AddRabbitGlobPatternExchange("*", "amq.direct");

var excludeExtensions = new[] { "html", "ico", "png", "jpg" };
builder.Services
    .AddOpenTelemetry()
    .WithTracing(builder => builder
        .SetResourceBuilder(ResourceBuilder.CreateDefault()
            .AddEnvironmentVariableDetector())
        .AddSource("Tgs.*")
        .AddSource("TGS.*")
        .AddSource("Valhalla.*")
        .AddAspNetCoreInstrumentation(options => options.Filter =
        httpContext => !httpContext.Request.Path.HasValue
            || !httpContext.Request.Path.StartsWithSegments("/swagger")
            && !httpContext.Request.Path.StartsWithSegments("/healthz")
            && !httpContext.Request.Path.StartsWithSegments("/profiler/")
            && !httpContext.Request.Path.StartsWithSegments("/hangfire")
            && !httpContext.Request.Path.StartsWithSegments("/metrics")
            && !excludeExtensions.Any(
                ext => httpContext.Request.Path.Value.EndsWith($".{ext}")))
        .AddHttpClientInstrumentation()
        .AddEntityFrameworkCoreInstrumentation(options => options.SetDbStatementForText = true)
        .AddOtlpExporter())
    .WithMetrics(meterBuiler => meterBuiler
        .SetResourceBuilder(ResourceBuilder.CreateDefault()
            .AddEnvironmentVariableDetector())
        .AddRuntimeInstrumentation()
        .AddAspNetCoreInstrumentation()
        .AddOtlpExporter())
    .Services
    .AddHealthChecks();

var app = builder.Build();

app.UseForwardedHeaders();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "SmsService API");
});

app.MapControllers();
app.MapHealthChecks("/healthz");

app.Run();
