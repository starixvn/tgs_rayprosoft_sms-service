using System;
using TGS.Sms;
using TGS.Sms.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static ReceiverConfiguration AddSmsReceiver(this IServiceCollection services)
        {
            var configuration = new ReceiverConfiguration(services);

            services
                .AddOptions<SmsOptions>()
                .Services
                .AddSingleton<ISmsContentConsumer, RabbitMQSmsContentConsumer>()
                .AddSingleton<RabbitMQManager>()
                .AddHostedService<SmsContentConsumerBackgroundService>();

            return configuration;
        }
    }
}
