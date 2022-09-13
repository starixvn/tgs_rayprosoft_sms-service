namespace TGS.SmsService.Client;

public interface ISmsServiceClient
{
    ValueTask SendShortMessageAsync(string phone, string message, Guid brandId);
}
