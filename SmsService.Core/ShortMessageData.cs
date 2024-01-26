namespace Tgs.SmsService;
public record ShortMessageData(
    Guid BrandId,
    string Destination,
    string Message,
    string? AreaCode,
    string? Phone);
