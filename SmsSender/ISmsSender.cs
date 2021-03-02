using System.Threading.Tasks;

namespace TGS.Sms
{
    public interface ISmsSender
    {
        ValueTask SendSmsAsync(string currency, string destination, string message);
    }
}
