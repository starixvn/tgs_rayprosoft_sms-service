using System.Threading.Tasks;

namespace Tgs.SmsService
{
    public interface ISmsSender
    {
        ValueTask SendSmsAsync(string vendorCode, string destination, string message);
    }
}
