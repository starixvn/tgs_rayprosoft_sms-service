using Telegram.Bot;
using Tgs.Sms.Protos;
using Valhalla.MessageQueue;

namespace Tgs.Sms.Mock.MessageQueue;

public class SmsMessageReceiveHandler(
    long chatId,
    ITelegramBotClient telegramBotClient)
    : IAcknowledgeMessageHandler<ShortMessage>
{

    public async ValueTask HandleAsync(IAcknowledgeMessage<ShortMessage> msg, CancellationToken cancellationToken = default)
    {
        var shortMessage = msg.Data!;
        var brandId = shortMessage.BrandId.ToGuid();

        _ = await telegramBotClient.SendTextMessageAsync(
            chatId,
            $"""
                【短信】
                Date: {DateTime.UtcNow}
                Destination: {shortMessage.Destination}
                BrandId: {brandId}
                AreaCode: {shortMessage.AreaCode}
                Phone: {shortMessage.Phone}
                Content: {shortMessage.Content}
                """,
            cancellationToken: cancellationToken).ConfigureAwait(false);

        await msg.AckAsync(cancellationToken).ConfigureAwait(false);
    }
}
