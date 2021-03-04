using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using TGS.SmsService.Metrics;
using OpenTelemetry.Trace;
using OpenTelemetry;

namespace TGS.SmsService
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            SmsApiMeter.Init();

            services.AddOpenTelemetryTracing(builder =>
                builder
                    .AddAspNetCoreInstrumentation()
                    .AddSource("TGS.*")
                    .AddProcessor(new SimpleActivityExportProcessor(new PerformanceCounterExporter())));

            services.AddControllers()
                .AddMvcOptions(options =>
                {
                    options.Filters.Add<ControllerDiagnosticsFilter>();
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
            });

            services.AddSmsServiceCore((options, sp) =>
            {
                var config = sp.GetRequiredService<IConfiguration>();

                config.GetSection("Sms").Bind(options);
            });

            services.AddHealthChecks();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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
    }
}
