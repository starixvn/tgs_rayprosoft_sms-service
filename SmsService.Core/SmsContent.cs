namespace TGS.SmsService
{
    public record SmsContent(
        string Destination,
        string Message,
        string Currency,
        string Brand);
}
