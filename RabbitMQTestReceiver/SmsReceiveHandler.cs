using Google.Protobuf;
using Tgs.MessageQueue;
using Tgs.Sms.Protos;

namespace TGS.RabbitMQTestReceiver;

internal class SmsReceiveHandler : IMessageHandler
{
    public ValueTask HandleAsync(ReadOnlyMemory<byte> data, CancellationToken cancellationToken = default)
    {
        var smsMessage = ShortMessage.Parser.ParseFrom(data.Span);

        Console.WriteLine(JsonFormatter.ToDiagnosticString(smsMessage));

        return ValueTask.CompletedTask;
    }
}
