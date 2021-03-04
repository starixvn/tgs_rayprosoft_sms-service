using System;
using TGS.Sms;
using TGS.SmsService;
using Valhalla.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSmsServiceCore(
            this IServiceCollection services,
            Action<SmsOptions, IServiceProvider> configureSmsSender)
        {
            return services
                .AddTransient<ICommandService<SmsContent>, SendSmsCommand>()
                .AddSmsSender(configureSmsSender);
        }
    }
}
