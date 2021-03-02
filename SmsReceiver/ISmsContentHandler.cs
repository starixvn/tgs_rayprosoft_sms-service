using System.Threading.Tasks;

namespace TGS.Sms
{
    public interface ISmsContentHandler
    {
        ValueTask HandleAsync(SmsContent smsContent);
    }
}
