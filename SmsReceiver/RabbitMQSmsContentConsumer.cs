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
        private IModel _channel;

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
            _channel = _rabbitMQManager.NotifyChannel!;

            _channel.QueueDeclare(
                queue: "sms",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += OnMessageReceivedAsync;

            _channel.BasicConsume(
                queue: "sms",
                autoAck: false,
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

                _channel.BasicAck(args.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Receive data fail!");
                _channel.BasicReject(args.DeliveryTag, true);
            }
        }
    }
}
