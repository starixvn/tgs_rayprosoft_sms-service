using System.Text.Json;
using Microsoft.EntityFrameworkCore;
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
            CreatedBy = modifier,
            CreatedOn = DateTime.UtcNow,
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
        smsSetting.UpdatedBy = modifier;
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

    public IAsyncEnumerable<SmsSettingInfo> GetSmsSettingsAsync(
        CancellationToken cancellationToken = default)
    {
        var smsSettings = _tgsContext.SmsSettings
            .AsNoTracking()
            .ToAsyncEnumerable();

        return smsSettings.Select(x => new SmsSettingInfo
        {
            SmsSettingId = x.Id,
            Name = x.Name,
            Code = x.Code,
            Setting = JsonDocument.Parse(x.Setting)
        });
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

    public async ValueTask AddBrandSmsAsync(
        Guid brandId,
        Guid smsSettingId,
        CancellationToken cancellationToken = default)
    {
        var brandSms = new BrandSmsRow
        {
            BrandId = brandId,
            SmsSettingId = smsSettingId,
        };

        _ = await _tgsContext.BrandSmsRows
            .AddAsync(brandSms, cancellationToken)
            .ConfigureAwait(false);

        _ = await _tgsContext
            .SaveChangesAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public async ValueTask RemoveBrandSmsAsync(
        Guid brandId,
        Guid smsSettingId, CancellationToken cancellationToken = default)
    {
        var brandSms = await _tgsContext.BrandSmsRows
            .FindAsync([brandId, smsSettingId], cancellationToken)
            .ConfigureAwait(false);

        if (brandSms is null)
            return;

        _tgsContext.BrandSmsRows.Remove(brandSms);

        _ = await _tgsContext
            .SaveChangesAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public IAsyncEnumerable<SmsSettingInfo> QueryBrandSmsListAsync(
        Guid brandId,
        CancellationToken cancellationToken = default)
    {
        var brandSms = from b in _tgsContext.BrandSmsRows
                       where b.BrandId == brandId
                       join s in _tgsContext.SmsSettings on b.SmsSettingId equals s.Id
                       select s;

        return brandSms.ToAsyncEnumerable()
            .Select(x => new SmsSettingInfo
            {
                SmsSettingId = x.Id,
                Name = x.Name,
                Code = x.Code,
                Setting  = JsonDocument.Parse(x.Setting)
            });
    }

    public async ValueTask ApplyBrandSmsAsync(
        Guid brandId,
        string vendorCode,
        JsonDocument vendorSetting,
        string modifier,
        CancellationToken cancellationToken = default)
    {
        var brand = await _tgsContext.Brands
            .FindAsync([brandId], cancellationToken)
            .ConfigureAwait(false);

        if (brand is null)
            return;

        brand.SmsVendorCode = vendorCode;
        brand.SmsVendorSettings = JsonSerializer.Serialize(vendorSetting);
        brand.UpdatedByUsername = modifier;
        brand.UpdatedOn = DateTime.UtcNow;

        await _tgsContext
            .SaveChangesAsync(cancellationToken)
            .ConfigureAwait(false);
    }
}