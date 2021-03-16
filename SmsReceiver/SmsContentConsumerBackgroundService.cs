using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace TGS.SmsReceiver
{
    internal class SmsContentConsumerBackgroundService : BackgroundService
    {
        private readonly ISmsContentConsumer _smsContentConsumer;

        public SmsContentConsumerBackgroundService(ISmsContentConsumer smsContentConsumer)
        {
            _smsContentConsumer = smsContentConsumer ?? throw new ArgumentNullException(nameof(smsContentConsumer));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _smsContentConsumer.StartReceiveAsync().ConfigureAwait(false);
        }
    }
}
