using Tgs.ServiceUtility;

namespace Tgs.SmsService.ApplicationServices;

internal class GetSmsSettingsQueryService(ITgsRepository repository)
    : IQueryService<GetSmsSettingsQuery, SmsSettingInfo>
{
    public IAsyncEnumerable<SmsSettingInfo> QueryAsync(GetSmsSettingsQuery query, CancellationToken cancellationToken = default)
        => repository.GetSmsSettingsAsync(cancellationToken);
}