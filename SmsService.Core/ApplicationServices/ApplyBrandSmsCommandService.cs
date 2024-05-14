using Tgs.ServiceUtility;

namespace Tgs.SmsService.ApplicationServices;

internal class ApplyBrandSmsCommandService(ITgsRepository repository)
    : ICommandService<ApplyBrandSmsCommand>
{
    public ValueTask ExecuteAsync(ApplyBrandSmsCommand command, CancellationToken cancellationToken = default)
        => repository.ApplyBrandSmsAsync(command.BrandId, command.SmsSettingId, command.Modifier, cancellationToken);
}