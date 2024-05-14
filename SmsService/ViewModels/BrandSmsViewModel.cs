namespace TGS.SmsService.ViewModels;

public record BrandSmsViewModel
{
    public Guid BrandId { get; set; }
    public Guid SmsSettingId { get; set; }
}