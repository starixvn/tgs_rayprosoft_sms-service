using System.Text.Json;

namespace Tgs.SmsService;

public record SmsSettingInfo
{
    public Guid SmsSettingId { get; init; }
    public string Name { get; init; } = default!;
    public string Code { get; init; } = default!;
    public JsonDocument Setting { get; init; } = default!;
};