using System;
using System.Threading.Tasks;
using Google.Protobuf;
using TGS.Sms.Protos;

namespace TGS.SmsSender
{
    internal class RabbitMQSender : ISmsSender
    {
        private readonly RabbitMQManager _rabbitMQManager;

        public RabbitMQSender(RabbitMQManager rabbitMQManager)
        {
            _rabbitMQManager = rabbitMQManager ?? throw new ArgumentNullException(nameof(rabbitMQManager));
        }

        public ValueTask SendSmsAsync(
            Guid brandId,
            string currency,
            string destination,
            string message)
        {
            var body = new ShortMessage()
            {
                Currency = currency,
                Destination = destination,
                Content = message,
                Brand = brandId.ToString()
            };

            _rabbitMQManager.SmsChannel!.BasicPublish(
                exchange: "amq.direct",
                routingKey: $"sms.{currency.ToLower()}",
                mandatory: true,
                basicProperties: null,
                body: body.ToByteArray().AsMemory());

            return ValueTask.CompletedTask;
        }
    }
}
