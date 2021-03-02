using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGS.Sms
{
    public record SmsContent(
        string Currency,
        string Destination,
        string Message);
}
