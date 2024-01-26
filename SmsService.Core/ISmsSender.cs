namespace Tgs.SmsService;

public interface ISmsSender
{
    ValueTask SendSmsAsync(string vendorCode, ShortMessageData message, CancellationToken cancellationToken = default);
}
