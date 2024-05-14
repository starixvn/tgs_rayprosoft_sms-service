using System.Text.Json;

namespace TGS.SmsService.ViewModels;

public class SmsSettingViewModel
{
    public required Guid SmsSettingId { get; set; }
    public required string Name { get; set; }
    public required string Code { get; set; }
    public required JsonDocument Setting { get; set; }
}