using System;
using System.Threading.Tasks;

namespace TGS.Sms
{
    public interface ISmsSender
    {
        ValueTask SendSmsAsync(Guid brandId, string currency, string destination, string message);
    }
}
