using System.Text.Json;

namespace Tgs.SmsService.ApplicationServices
{
    public record SendSmsCommand(
        string Destination,
        string Message,
        string VendorCode,
        JsonDocument VendorSettings);
}
