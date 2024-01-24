using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TGS.RabbitMQTestReceiver;

internal class Program
{
    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                services
                    .AddMessageQueue()
                    .AddRabbitMessageQueue(config => config
                        .ConfigQueueOptions((options, sp) =>
                        {
                            var configuration = sp.GetRequiredService<IConfiguration>();

                            configuration.GetSection("Sms").Bind(options);

                            options.OnSetupQueueAndExchange = channel =>
                            {
                                channel.QueueDeclare(
                                    queue: "sms.test",
                                    durable: true,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

                                channel.QueueBind(
                                    "sms.test",
                                    "amq.direct",
                                    $"tgs.sms.test",
                                    new Dictionary<string, object>());
                            };
                        })
                        .AddHandler<SmsReceiveHandler>("sms.test", false))
                    .AddRabbitGlobPatternExchange("*", "amq.direct");
            });
    }

    private static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }
}
