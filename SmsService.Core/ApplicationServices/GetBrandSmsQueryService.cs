using Tgs.ServiceUtility;

namespace Tgs.SmsService.ApplicationServices;

internal class GetBrandSmsQueryService(ITgsRepository repository)
    : IQueryService<GetBrandSmsQuery, BrandSmsInfo>
{
    public IAsyncEnumerable<BrandSmsInfo> QueryAsync(GetBrandSmsQuery query, CancellationToken cancellationToken = default)
        => repository.QueryBrandSmsListAsync(query.BrandId, cancellationToken);
}