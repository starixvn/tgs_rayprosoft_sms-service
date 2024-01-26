using System.Reflection;
using Tgs.SmsService;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSmsServiceCore(
        this IServiceCollection services)
    {
        return services
            .RegisterAllServiceUtilityTypes(
                Assembly.GetExecutingAssembly(),
                ServiceLifetime.Transient)
            .AddTransient<ISmsSender, MessageQueueSender>();
    }
}
