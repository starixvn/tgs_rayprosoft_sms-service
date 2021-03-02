using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGS.Sms;

namespace RabbitMQTestReceiver
{
    class SmsReceiveHandler : ISmsContentHandler
    {
        public ValueTask HandleAsync(SmsContent smsContent)
        {
            throw new NotImplementedException();
        }
    }
}
