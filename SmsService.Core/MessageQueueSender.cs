using System;
using System.Threading.Tasks;
using Google.Protobuf;
using Tgs.MessageQueue;
using Tgs.Sms.Protos;

namespace Tgs.SmsService
{
    internal class MessageQueueSender : ISmsSender
    {
        private readonly IMessageSender _messageSender;

        public MessageQueueSender(IMessageSender messageSender)
        {
            _messageSender = messageSender ?? throw new ArgumentNullException(nameof(messageSender));
        }

        public ValueTask SendSmsAsync(string vendorCode, string destination, string message)
        {
            var body = new ShortMessage()
            {
                Destination = destination,
                Content = message
            };

            return _messageSender
                .PublishAsync($"tgs.sms.{vendorCode}", body.ToByteArray());
        }
    }
}
