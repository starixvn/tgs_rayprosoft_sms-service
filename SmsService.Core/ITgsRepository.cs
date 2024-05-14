using System.Text.Json;

namespace Tgs.SmsService;

public interface ITgsRepository
{
    public ValueTask<BrandInfo?> GetBrandInfoByIdAsync(Guid brandId);

    public ValueTask AddSmsSettingAsync(
        string name,
        string code,
        JsonDocument setting,
        string modifier,
        CancellationToken cancellationToken = default);

    public ValueTask UpdateSmsSettingAsync(
        Guid smsSettingId,
        string name,
        string code,
        JsonDocument setting,
        string modifier,
        CancellationToken cancellationToken = default);

    public ValueTask RemoveSmsSettingAsync(
        Guid smsSettingId,
        CancellationToken cancellationToken = default);

    public IAsyncEnumerable<SmsSettingInfo> GetSmsSettingsAsync(
        CancellationToken cancellationToken = default);

    public ValueTask<SmsSettingInfo?> GetSmsSettingByIdAsync(
        Guid smsSettingId,
        CancellationToken cancellationToken = default);
}