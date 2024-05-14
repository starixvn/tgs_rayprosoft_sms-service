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

    ValueTask<SmsSettingInfo?> GetSmsSettingAsync(
        Guid SmsSettingId,
        CancellationToken cancellationToken = default);

    ValueTask AddSmsSettingAsync(
        string name,
        string code,
        JsonDocument setting,
        string modifier,
        CancellationToken cancellationToken = default);

    ValueTask UpdateSmsSettingAsync(
        Guid SmsSettingId,
        string name,
        string code,
        JsonDocument setting,
        string modifier,
        CancellationToken cancellationToken = default);

    ValueTask RemoveSmsSettingAsync(
        Guid SmsSettingId,
        CancellationToken cancellationToken = default);
}