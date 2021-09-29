using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using OpenTelemetry;
using OpenTelemetry.Trace;
using Tgs.WebApi.Toolkits;
using TGS.SmsService.Metrics;

namespace TGS.SmsService
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseForwardedHeaders();

            app
                .UseSwagger()
                .UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "SmsService API");
                });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            SmsApiMeter.Init();

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.KnownNetworks.Add(new IPNetwork(IPAddress.Parse("172.0.0.0"), 8));
                options.KnownNetworks.Add(new IPNetwork(IPAddress.Parse("10.0.0.0"), 8));
                options.ForwardedHeaders = ForwardedHeaders.All;
            });

            services.AddOpenTelemetryTracing(builder =>
                builder
                    .AddAspNetCoreInstrumentation()
                    .AddSource("TGS.*")
                    .AddSource("Tgs.*")
                    .AddProcessor(new SimpleActivityExportProcessor(new PerformanceCounterExporter())));

            services.AddControllers()
                .AddMvcOptions(options =>
                {
                    options.Filters.Add<ControllerDiagnosticsFilter>();
                    options.Filters.Add<RetouchResponseFilter>();
                });

            services.AddSwaggerGen(options =>
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

            services
                .AddSmsServiceCore()
                .AddSmsServiceRepositories((sp, dbContextBuilder) =>
                {
                    var config = sp.GetRequiredService<IConfiguration>();

                    dbContextBuilder.UseNpgsql(config.GetConnectionString("Tgs"));

                })
                .AddRabbitMessageQueue(config =>
                {
                    config
                        .ConfigQueueOptions((options, sp) =>
                        {
                            var configuration = sp.GetRequiredService<IConfiguration>();

                            configuration.GetSection("Sms").Bind(options);
                        })
                        .ProcessSubjectPattern("*", "amq.direct");
                });

            services.AddHealthChecks();
        }
    }
}
