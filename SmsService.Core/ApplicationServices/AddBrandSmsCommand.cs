namespace Tgs.SmsService.ApplicationServices;

public record AddBrandSmsCommand(
    Guid BrandId,
    Guid SmsSettingId);