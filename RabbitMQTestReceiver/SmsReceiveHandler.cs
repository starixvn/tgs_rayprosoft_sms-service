using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TGS.Sms;

namespace RabbitMQTestReceiver
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
