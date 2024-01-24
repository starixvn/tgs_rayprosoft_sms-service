namespace TGS.SmsService.Client;

public interface ISmsServiceClient
{
    ValueTask SendShortMessageAsync(
        string phone,
        string message,
        Guid brandId,
        string? areaCode = null,
        CancellationToken cancellationToken = default);
}
