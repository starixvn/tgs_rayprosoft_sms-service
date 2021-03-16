using System.Threading.Tasks;

namespace TGS.SmsReceiver
{
    internal interface ISmsContentConsumer
    {
        ValueTask StartReceiveAsync();
    }
}