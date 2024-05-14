using Tgs.ServiceUtility;

namespace Tgs.SmsService.ApplicationServices;

internal class RemoveSmsSettingCommandService(ITgsRepository repository)
    : ICommandService<RemoveSmsSettingCommand>
{
    public ValueTask ExecuteAsync(RemoveSmsSettingCommand command, CancellationToken cancellationToken = default)
        => repository.RemoveSmsSettingAsync(command.SmsSettingId, cancellationToken);
}