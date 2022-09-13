namespace Tgs.SmsService;

public interface ITgsRepository
{
    public ValueTask<BrandInfo?> GetBrandInfoByIdAsync(Guid brandId);
}
