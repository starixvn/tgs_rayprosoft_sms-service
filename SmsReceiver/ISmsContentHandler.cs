using System.Threading.Tasks;

namespace TGS.SmsReceiver
{
    public interface ISmsContentHandler
    {
        ValueTask HandleAsync(SmsContent smsContent);
    }
}
