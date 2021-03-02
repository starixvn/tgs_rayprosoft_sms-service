using System;
using System.Threading.Tasks;
using Google.Protobuf;
using TGS.Sms.Protos;

namespace TGS.Sms
{
    internal class RabbitMQSender : ISmsSender
    {
        private readonly RabbitMQManager _rabbitMQManager;

        public RabbitMQSender(RabbitMQManager rabbitMQManager)
        {
            _rabbitMQManager = rabbitMQManager ?? throw new ArgumentNullException(nameof(rabbitMQManager));
        }

        public ValueTask SendSmsAsync(string currency, string destination, string message)
        {
            var body = new ShortMessage()
            {
                Currency = currency,
                Destination = destination,
                Content = message
            };

            _rabbitMQManager.NotifyChannel!.BasicPublish(
                exchange: string.Empty,
                routingKey: "sms",
                mandatory: true,
                basicProperties: null,
                body: body.ToByteArray().AsMemory());

            return ValueTask.CompletedTask;
        }
    }
}
