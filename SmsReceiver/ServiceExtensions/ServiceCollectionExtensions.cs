using TGS.SmsReceiver;
using TGS.SmsReceiver.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static ReceiverConfiguration AddSmsReceiver(this IServiceCollection services)
        {
            var configuration = new ReceiverConfiguration(services);

            services
                .AddOptions<MessageQueueOptions>()
                .Services
                .AddSingleton<ISmsContentConsumer, RabbitMQSmsContentConsumer>()
                .AddSingleton<RabbitMQManager>()
                .AddHostedService<SmsContentConsumerBackgroundService>();

            return configuration;
        }
    }
}
