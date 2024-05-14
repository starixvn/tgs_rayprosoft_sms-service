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

    public ValueTask AddSmsSettingAsync(
        string name,
        string code,
        JsonDocument setting,
        string modifier,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public ValueTask UpdateSmsSettingAsync(
        Guid smsSettingId,
        string name,
        string code,
        JsonDocument setting,
        string modifier,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public ValueTask RemoveSmsSettingAsync(
        Guid smsSettingId,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<SmsSettingInfo> GetSmsSettingsAsync(
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public ValueTask<SmsSettingInfo?> GetSmsSettingByIdAsync(
        Guid smsSettingId,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public ValueTask AddBrandSmsAsync(
        Guid brandId, 
        Guid smsSettingId, 
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public ValueTask RemoveBrandSmsAsync(
        Guid brandId, 
        Guid smsSettingId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<BrandSmsInfo> QueryBrandSmsListAsync(
        Guid brandId, 
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public ValueTask ApplyBrandSmsAsync(
        Guid brandId, 
        Guid smsSettingId, 
        string modifier, 
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}