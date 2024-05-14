using Tgs.ServiceUtility;

namespace Tgs.SmsService.ApplicationServices;

internal class GetSmsSettingQueryService(ITgsRepository repository) : IGetService<GetSmsSettingQuery, SmsSettingInfo?>
{
    public ValueTask<SmsSettingInfo?> GetAsync(GetSmsSettingQuery query, CancellationToken cancellationToken = default)
    => repository.GetSmsSettingByIdAsync(query.SmsSettingId, cancellationToken);
}