namespace Tgs.SmsService;

public interface ISmsSender
{
    ValueTask SendSmsAsync(Guid brandId, string vendorCode, string destination, string message);
}
