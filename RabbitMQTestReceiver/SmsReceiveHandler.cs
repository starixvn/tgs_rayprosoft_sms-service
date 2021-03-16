using System;
using System.Text.Json;
using System.Threading.Tasks;
using TGS.Sms;
using TGS.SmsReceiver;

namespace TGS.RabbitMQTestReceiver
{
    class SmsReceiveHandler : ISmsContentHandler
    {
        public ValueTask HandleAsync(SmsContent smsContent)
        {
            Console.WriteLine(JsonSerializer.Serialize(smsContent));

            return ValueTask.CompletedTask;
        }
    }
}
