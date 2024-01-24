using TGS.SmsService.Client;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSmsClient(this IServiceCollection services, Action<IServiceProvider, HttpClient> configureSmsHttpClient)
    {
        return services
            .AddHttpClient<SmsServiceClient>()
            .ConfigureHttpClient(configureSmsHttpClient)
            .Services
            .AddTransient<ISmsServiceClient>(sp => sp.GetRequiredService<SmsServiceClient>());
    }
}
