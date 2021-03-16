using System;
using Microsoft.Extensions.DependencyInjection;

namespace TGS.SmsReceiver.Configuration
{
    public class ReceiverConfiguration
    {
        public IServiceCollection Services { get; }

        public ReceiverConfiguration(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }

        public ReceiverConfiguration RegisterHandler<THandler>() where THandler : class, ISmsContentHandler
        {
            Services.AddSingleton<ISmsContentHandler, THandler>();

            return this;
        }

        public ReceiverConfiguration ConfigureOptions(Action<SmsOptions, IServiceProvider> configure)
        {
            Services
                .AddOptions<SmsOptions>()
                .Configure(configure);

            return this;
        }
    }
}
