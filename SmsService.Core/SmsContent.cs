using System;

namespace TGS.SmsService.Core
{
    public record SmsContent(
        string Destination,
        string Message,
        string Currency,
        Guid BrandId);
}
