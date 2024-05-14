namespace Tgs.SmsService.ApplicationServices;

public record RemoveBrandSmsCommand(
    Guid BrandId,
    Guid SmsSettingId);