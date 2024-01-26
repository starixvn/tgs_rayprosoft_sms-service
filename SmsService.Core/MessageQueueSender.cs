using Google.Protobuf;
using Tgs.Sms.Protos;
using Valhalla.MessageQueue;

namespace Tgs.SmsService;

internal class MessageQueueSender(IMessageSender messageSender) : ISmsSender
{
    public ValueTask SendSmsAsync(string vendorCode, ShortMessageData message, CancellationToken cancellationToken = default)
    {
        var body = new ShortMessage()
        {
            BrandId = message.BrandId,
            Destination = message.Destination,
            Content = message.Message,
            AreaCode = message.AreaCode ?? string.Empty,
            Phone = message.Phone ?? string.Empty
        };

        return messageSender.PublishAsync(
            $"tgs.sms.{vendorCode}",
            body.ToByteArray(),
            cancellationToken: cancellationToken);
    }
}
