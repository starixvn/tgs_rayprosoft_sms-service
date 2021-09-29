using System;
using System.Text.Json;

namespace Tgs.SmsService.ApplicationServices
{
    public record SendSmsCommand(
        Guid BrandId,
        string Destination,
        string Message,
        string VendorCode,
        JsonDocument VendorSettings);
}
