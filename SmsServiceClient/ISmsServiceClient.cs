using System;
using System.Threading.Tasks;

namespace TGS.SmsService.Client
{
    public interface ISmsServiceClient
    {
        ValueTask SendShortMessageAsync(string phone, string message, string currency, Guid brandId);
    }
}
