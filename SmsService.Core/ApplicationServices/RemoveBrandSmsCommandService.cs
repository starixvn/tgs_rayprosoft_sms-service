using Tgs.ServiceUtility;

namespace Tgs.SmsService.ApplicationServices;

internal class RemoveBrandSmsCommandService(ITgsRepository repository) : ICommandService<RemoveBrandSmsCommand>
{
    public ValueTask ExecuteAsync(RemoveBrandSmsCommand command, CancellationToken cancellationToken = default)
        => repository.RemoveBrandSmsAsync(command.BrandId, command.SmsSettingId, cancellationToken);
}