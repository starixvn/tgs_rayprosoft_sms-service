using System;
using TGS.Sms;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSmsSender(
            this IServiceCollection services,
            Action<SmsOptions, IServiceProvider> configure)
        {
            return services
                .AddOptions<SmsOptions>()
                .Configure(configure)
                .Services
                .AddTransient<ISmsSender, RabbitMQSender>()
                .AddSingleton<RabbitMQManager>();
        }

        public static IServiceCollection AddSmsSender(
            this IServiceCollection services,
            Action<SmsOptions> configure)
        {
            return services
                .AddOptions<SmsOptions>()
                .Configure(configure)
                .Services
                .AddTransient<ISmsSender, RabbitMQSender>()
                .AddSingleton<RabbitMQManager>();
        }
    }
}
