using System;
using System.Net.Http;
using TGS.SmsService.Client;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSmsClient(this IServiceCollection services, Action<HttpClient> configureSmsHttpClient) =>
            services
                .AddHttpClient<SmsServiceClient>()
                .ConfigureHttpClient(configureSmsHttpClient)
                .Services
                .AddTransient<ISmsServiceClient, SmsServiceClient>();
    }
}
