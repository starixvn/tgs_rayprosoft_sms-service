using System;
using System.Text.Json;

namespace Tgs.SmsService
{
    public record BrandInfo(
        Guid Id,
        bool SmsEnabled,
        string? SmsVendorCode,
        JsonDocument? SmsVendorSettings);
}
