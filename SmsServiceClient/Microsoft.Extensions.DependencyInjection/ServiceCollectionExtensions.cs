using TGS.SmsService.Client;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSmsClient(this IServiceCollection services) =>
            services
                .AddTransient<ISmsServiceClient, SmsServiceClient>();
    }
}
