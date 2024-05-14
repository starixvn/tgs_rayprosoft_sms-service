using System.Text.Json;

namespace Tgs.SmsService.ApplicationServices;

public record AddSmsSettingCommand(
    string Name,
    string Code,
    JsonDocument Setting,
    string Modifier);