namespace TGS.SmsService.Client;

public interface ISmsServiceClient
{
    ValueTask SendShortMessageAsync(
        string destination,
        string message,
        Guid brandId,
        string? areaCode = null,
        string? phone = null,
        CancellationToken cancellationToken = default);
}
