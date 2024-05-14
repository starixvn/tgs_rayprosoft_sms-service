using System.Text.Json;
using Tgs.Sms.Repositories.Entities;
using Tgs.SmsService;

namespace SmsService.Repositories;

internal class TgsRepository(TgsContext tgsContext) : ITgsRepository
{
    private readonly TgsContext _tgsContext = tgsContext ?? throw new ArgumentNullException(nameof(tgsContext));

    public async ValueTask<BrandInfo?> GetBrandInfoByIdAsync(Guid brandId)
    {
        var brand = await _tgsContext.Brands
            .FindAsync(brandId)
            .ConfigureAwait(false);

        if (brand == null)
        {
            return null;
        }

        return new BrandInfo(
            brand.Id,
            brand.EnableSms,
            brand.SmsVendorCode,
            string.IsNullOrWhiteSpace(brand.SmsVendorSettings)
                ? null
                : JsonDocument.Parse(brand.SmsVendorSettings));
    }

    public async ValueTask AddSmsSettingAsync(
        string name,
        string code,
        JsonDocument setting,
        string modifier,
        CancellationToken cancellationToken = default)
    {
        var brandSetting = new SmsSetting
        {
            Name = name,
            Code = code,
            Setting = JsonSerializer.Serialize(setting),
            UpdatedByUsername = modifier,
            UpdatedOn = DateTime.UtcNow,
        };

        _ = await _tgsContext.SmsSettings
            .AddAsync(brandSetting, cancellationToken)
            .ConfigureAwait(false);

        _ = await _tgsContext
            .SaveChangesAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public async ValueTask UpdateSmsSettingAsync(
        Guid smsSettingId,
        string name,
        string code,
        JsonDocument setting,
        string modifier,
        CancellationToken cancellationToken = default)
    {
        var smsSetting = await _tgsContext.SmsSettings
            .FindAsync([smsSettingId], cancellationToken)
            .ConfigureAwait(false);

        if (smsSetting is null)
            return;

        smsSetting.Name = name;
        smsSetting.Code = code;
        smsSetting.Setting = JsonSerializer.Serialize(setting);
        smsSetting.UpdatedByUsername = modifier;
        smsSetting.UpdatedOn = DateTime.UtcNow;

        _ = await _tgsContext
            .SaveChangesAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public async ValueTask RemoveSmsSettingAsync(
        Guid smsSettingId,
        CancellationToken cancellationToken = default)
    {
        var smsSetting = await _tgsContext.SmsSettings
            .FindAsync([smsSettingId], cancellationToken)
            .ConfigureAwait(false);

        if (smsSetting is null)
            return;

        _ = _tgsContext.SmsSettings.Remove(smsSetting);

        _ = await _tgsContext
            .SaveChangesAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public IAsyncEnumerable<SmsSettingInfo> GetSmsSettingsAsync(CancellationToken cancellationToken = default)
    {
        var smsSettings = _tgsContext.SmsSettings
            .ToAsyncEnumerable();

        return smsSettings.Select(x => new SmsSettingInfo
        {
            SmsSettingId = x.Id,
            Name = x.Name,
            Code = x.Code,
            Setting = JsonDocument.Parse(x.Setting)
        }).AsAsyncEnumerable();
    }

    public async ValueTask<SmsSettingInfo?> GetSmsSettingByIdAsync(
        Guid smsSettingId,
        CancellationToken cancellationToken = default)
    {
        var smsSetting = await _tgsContext.SmsSettings
            .FindAsync([smsSettingId], cancellationToken)
            .ConfigureAwait(false);

        if (smsSetting is null)
            return null;

        return new SmsSettingInfo
        {
            SmsSettingId = smsSetting.Id,
            Name = smsSetting.Name,
            Code = smsSetting.Code,
            Setting = JsonDocument.Parse(smsSetting.Setting)
        };
    }
}