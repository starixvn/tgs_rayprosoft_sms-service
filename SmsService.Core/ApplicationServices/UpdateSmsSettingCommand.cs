using System.Text.Json;

namespace Tgs.SmsService.ApplicationServices;

public record UpdateSmsSettingCommand(
    Guid SmsSettingId,
    string Name,
    string Code,
    JsonDocument Setting,
    string Modifier);