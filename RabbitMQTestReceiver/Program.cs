using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace RabbitMQTestReceiver
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
                            options.RabbitMQUrl = new Uri("amqp://localhost:5672");
                        })
                        .RegisterHandler<SmsReceiveHandler>();
                });

        private static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
    }
}
