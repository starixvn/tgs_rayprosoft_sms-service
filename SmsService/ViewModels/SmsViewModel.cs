namespace TGS.SmsService.ViewModels
{
    public record SmsViewModel(
        string Destination,
        string Message,
        string Currency);
}
