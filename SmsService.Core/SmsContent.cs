using System;

namespace TGS.SmsService
{
    public record SmsContent(
        string Destination,
        string Message,
        string Currency,
        Guid BrandId);
}
