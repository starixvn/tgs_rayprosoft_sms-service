using System;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf;
using Tgs.MessageQueue;
using Tgs.Sms.Protos;

namespace TGS.RabbitMQTestReceiver
{
    internal class SmsReceiveHandler : IMessageHandler
    {
        public ValueTask HandleAsync(byte[] data, CancellationToken cancellationToken = default)
        {
            var smsMessage = ShortMessage.Parser.ParseFrom(data);

            Console.WriteLine(JsonFormatter.ToDiagnosticString(smsMessage));

            return ValueTask.CompletedTask;
        }
    }
}
