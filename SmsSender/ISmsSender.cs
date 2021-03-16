using System;
using System.Threading.Tasks;

namespace TGS.SmsSender
{
    public interface ISmsSender
    {
        ValueTask SendSmsAsync(Guid brandId, string currency, string destination, string message);
    }
}
