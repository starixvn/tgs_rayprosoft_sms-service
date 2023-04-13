using System.Net;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OpenTelemetry;
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Tgs.WebApi.Toolkits;
using TGS.SmsService;
using TGS.SmsService.Metrics;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("/config/appsettings.json", true)
    .AddEnvironmentVariables();

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
            fluentOptions.UseMessagePack();
        });
    });


SmsApiMeter.Init();

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.KnownNetworks.Add(new IPNetwork(IPAddress.Parse("172.0.0.0"), 8));
    options.KnownNetworks.Add(new IPNetwork(IPAddress.Parse("10.0.0.0"), 8));
    options.ForwardedHeaders = ForwardedHeaders.All;
});

builder.Services
    .AddOpenTelemetry()
    .WithTracing(builder => builder
        .SetResourceBuilder(ResourceBuilder.CreateDefault()
            .AddEnvironmentVariableDetector())
        .AddSource("Tgs.*")
        .AddSource("TGS.*")
        .AddAspNetCoreInstrumentation()
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
    .Configure<AspNetCoreInstrumentationOptions>(options => options.Filter =
        httpContext => !httpContext.Request.Path.HasValue
            || !httpContext.Request.Path.StartsWithSegments("/swagger")
            && !httpContext.Request.Path.StartsWithSegments("/healthz")
            && !httpContext.Request.Path.StartsWithSegments("/profiler/")
            && !httpContext.Request.Path.StartsWithSegments("/hangfire")
            && !httpContext.Request.Path.StartsWithSegments("/metrics")
            && !new[] { "html", "ico", "png", "jpg" }.Any(
                ext => httpContext.Request.Path.Value.EndsWith($".{ext}")));

builder.Services.AddControllers()
    .AddMvcOptions(options =>
    {
        options.Filters.Add<ControllerDiagnosticsFilter>();
        options.Filters.Add<RetouchResponseFilter>();
    });

builder.Services.AddSwaggerGen(options =>
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
    .AddMessageQueue()
    .AddRabbitMessageQueue(config => config
        .ConfigQueueOptions((options, sp) =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();

            configuration.GetSection("Sms").Bind(options);
        }))
    .AddRabbitGlobPatternExchange("*", "amq.direct");

builder.Services.AddHealthChecks();

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
