using Tgs.ServiceUtility;

namespace Tgs.SmsService.ApplicationServices;

internal class AddSmsSettingCommandService(ITgsRepository repository)
    : ICommandService<AddSmsSettingCommand>
{
    public ValueTask ExecuteAsync(AddSmsSettingCommand command, CancellationToken cancellationToken = default)
        => repository.AddSmsSettingAsync(
                command.Name,
                command.Code,
                command.Setting,
                command.Modifier,
                cancellationToken);
}