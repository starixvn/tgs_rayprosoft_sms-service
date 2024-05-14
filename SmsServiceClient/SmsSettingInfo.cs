using System.Text.Json;

namespace TGS.SmsService.Client;

public record SmsSettingInfo(
    Guid SmsSettingId,
    string Name,
    string Code,
    JsonDocument Setting);