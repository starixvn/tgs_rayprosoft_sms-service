using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using TGS.Sms.Protos;

namespace TGS.Sms
{
    internal class RabbitMQSmsContentConsumer : ISmsContentConsumer
    {
        private readonly RabbitMQManager _rabbitMQManager;
        private readonly ISmsContentHandler _handler;
        private readonly ILogger<RabbitMQSmsContentConsumer> _logger;

        public RabbitMQSmsContentConsumer(
            RabbitMQManager rabbitMQManager,
            ISmsContentHandler handler,
            ILogger<RabbitMQSmsContentConsumer> logger)
        {
            _rabbitMQManager = rabbitMQManager ?? throw new ArgumentNullException(nameof(rabbitMQManager));
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public ValueTask StartReceiveAsync()
        {
            var channel = _rabbitMQManager.NotifyChannel;

            channel!.QueueDeclare(
                queue: "sms",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += OnMessageReceivedAsync;

            channel.BasicConsume(
                queue: "sms",
                autoAck: true,
                consumer: consumer);

            return ValueTask.CompletedTask;
        }

        private async Task OnMessageReceivedAsync(object model, BasicDeliverEventArgs args)
        {
            try
            {
                var shortMessage = ShortMessage.Parser.ParseFrom(args.Body.ToArray());

                await _handler
                    .HandleAsync(new SmsContent(shortMessage.Currency, shortMessage.Destination, shortMessage.Content))
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Receive data fail!");
            }
        }
    }
}
