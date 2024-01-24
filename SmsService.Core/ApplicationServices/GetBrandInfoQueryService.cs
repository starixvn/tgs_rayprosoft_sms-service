using Tgs.ServiceUtility;

namespace Tgs.SmsService.ApplicationServices;

internal class GetBrandInfoQueryService(ITgsRepository tgsRepository) : IGetService<GetBrandInfoQuery, BrandInfo?>
{
    private readonly ITgsRepository _tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));

    public ValueTask<BrandInfo?> GetAsync(GetBrandInfoQuery query, CancellationToken cancellationToken = default)
    {
        return _tgsRepository.GetBrandInfoByIdAsync(query.BrandId);
    }
}
