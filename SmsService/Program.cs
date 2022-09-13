using System.Net;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OpenTelemetry;
using OpenTelemetry.Trace;
using Tgs.WebApi.Toolkits;
using TGS.SmsService;
using TGS.SmsService.Metrics;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("/config/appsettings.json", true);

SmsApiMeter.Init();

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.KnownNetworks.Add(new IPNetwork(IPAddress.Parse("172.0.0.0"), 8));
    options.KnownNetworks.Add(new IPNetwork(IPAddress.Parse("10.0.0.0"), 8));
    options.ForwardedHeaders = ForwardedHeaders.All;
});

builder.Services.AddOpenTelemetryTracing(builder =>
    builder
        .AddAspNetCoreInstrumentation()
        .AddSource("TGS.*")
        .AddSource("Tgs.*")
        .AddProcessor(new SimpleActivityExportProcessor(new PerformanceCounterExporter())));

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

    options.OperationFilter<SwaggerDocFilter>();
});

builder.Services
    .AddSmsServiceCore()
    .AddSmsServiceRepositories((sp, dbContextBuilder) =>
    {
        var config = sp.GetRequiredService<IConfiguration>();

        dbContextBuilder.UseNpgsql(config.GetConnectionString("Tgs"));
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
