namespace Tgs.SmsService.ApplicationServices;

public record ApplyBrandSmsCommand(
    Guid BrandId,
    Guid SmsSettingId,
    string Modifier);