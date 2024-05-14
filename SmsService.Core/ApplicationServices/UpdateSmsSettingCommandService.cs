using Tgs.ServiceUtility;

namespace Tgs.SmsService.ApplicationServices;

internal class UpdateSmsSettingCommandService(ITgsRepository repository)
    : ICommandService<UpdateSmsSettingCommand>
{
    public ValueTask ExecuteAsync(UpdateSmsSettingCommand command, CancellationToken cancellationToken = default)
        => repository.UpdateSmsSettingAsync(command.SmsSettingId, command.Name, command.Code, command.Setting, command.Modifier, cancellationToken);
}