using System;
using Microsoft.Extensions.DependencyInjection;
using TGS.ServiceUtility;
using TGS.SmsSender;
using TGS.SmsSender.ServiceExtensions;

namespace TGS.SmsService.Core.ServiceExtensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSmsServiceCore(
            this IServiceCollection services,
            Action<SmsOptions, IServiceProvider> configureSmsSender)
        {
            return services
                .AddTransient<ICommandService<SmsContent>, SendSmsCommandService>()
                .AddSmsSender(configureSmsSender);
        }
    }
}
