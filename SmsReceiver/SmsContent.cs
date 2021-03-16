namespace TGS.SmsReceiver
{
    public record SmsContent(
        string Currency,
        string Destination,
        string Message);
}
