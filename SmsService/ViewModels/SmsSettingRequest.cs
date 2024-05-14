using System.Text.Json;

namespace TGS.SmsService.ViewModels;

public record SmsSettingRequest
{
    public string Name { get; set; } = default!;
    public string Code { get; set; } = default!;

    public JsonDocument Setting { get; set; } = default!;
}
