using System.Threading.Tasks;

namespace TGS.Sms
{
    internal interface ISmsContentConsumer
    {
        ValueTask StartReceiveAsync();
    }
}