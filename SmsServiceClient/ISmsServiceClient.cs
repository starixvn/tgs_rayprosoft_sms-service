using System.Text.Json;

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

    IAsyncEnumerable<SmsSettingInfo> QuerySmsSettingListAsync(
        CancellationToken cancellationToken = default);

    ValueTask AddSmsSettingAsync(
        string name,
        string code,
        JsonDocument setting,
        string modifier,
        CancellationToken cancellationToken = default);

    ValueTask UpdateSmsSettingAsync(
        Guid smsSettingId,
        string name,
        string code,
        JsonDocument setting,
        string modifier,
        CancellationToken cancellationToken = default);

    ValueTask RemoveSmsSettingAsync(
        Guid smsSettingId,
        CancellationToken cancellationToken = default);

    ValueTask AddBrandSmsAsync(
        Guid brandId,
        Guid smsSettingId,
        CancellationToken cancellationToken = default);

    ValueTask RemoveBrandSmsAsyncAsync(
        Guid brandId,
        Guid smsSettingId,
        CancellationToken cancellationToken = default);

    IAsyncEnumerable<BrandSmsInfo> QueryBrandSmsListAsync(
        Guid brandId,
        CancellationToken cancellationToken = default);

    ValueTask ApplyBrandSmsAsync(
        Guid brandId,
        Guid smsSettingId,
        string modifier,
        CancellationToken cancellationToken = default);
}