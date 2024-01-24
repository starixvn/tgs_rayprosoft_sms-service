using Google.Protobuf;
using Tgs.Sms.Protos;
using Valhalla.MessageQueue;

namespace Tgs.SmsService;

internal class MessageQueueSender(IMessageSender messageSender) : ISmsSender
{
    private readonly IMessageSender _messageSender = messageSender ?? throw new ArgumentNullException(nameof(messageSender));

    public ValueTask SendSmsAsync(Guid brandId, string vendorCode, string destination, string message)
    {
        var body = new ShortMessage()
        {
            BrandId = brandId,
            Destination = destination,
            Content = message
        };

        return _messageSender
            .PublishAsync($"tgs.sms.{vendorCode}", body.ToByteArray());
    }
}
