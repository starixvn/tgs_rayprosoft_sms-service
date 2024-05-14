using Tgs.ServiceUtility;

namespace Tgs.SmsService.ApplicationServices;

internal class GetBrandSmsQueryService(ITgsRepository repository)
    : IQueryService<GetBrandSmsQuery, SmsSettingInfo>
{
    public IAsyncEnumerable<SmsSettingInfo> QueryAsync(GetBrandSmsQuery query, CancellationToken cancellationToken = default)
        => repository.QueryBrandSmsListAsync(query.BrandId, cancellationToken);
}