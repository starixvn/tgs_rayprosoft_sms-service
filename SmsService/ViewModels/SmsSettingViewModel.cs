using System.Text.Json;

namespace TGS.SmsService.ViewModels;

public record SmsSettingViewModel
{
    public Guid SmsSettingId { get; set; }
    public string Name { get; set; } = default!;
    public string Code { get; set; } = default!;
    public JsonDocument Setting { get; set; } = default!;
}