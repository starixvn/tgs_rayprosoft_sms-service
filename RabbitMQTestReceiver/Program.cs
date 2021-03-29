using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TGS.RabbitMQTestReceiver
{
    internal class Program
    {
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddSmsReceiver()
                        .ConfigureOptions((options, sp) =>
                        {
                            var config = sp.GetRequiredService<IConfiguration>();

                            config.GetSection("Sms").Bind(options);
                        })
                        .RegisterHandler<SmsReceiveHandler>();
                });

        private static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
    }
}
