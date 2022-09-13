namespace TGS.SmsService.ViewModels;

/// <summary>
/// The sms content view model.
/// </summary>
public record SmsViewModel(
    string Destination,
    string Message,
    Guid BrandId);
