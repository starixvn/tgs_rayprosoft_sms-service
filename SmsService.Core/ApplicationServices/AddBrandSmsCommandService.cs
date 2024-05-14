using Tgs.ServiceUtility;

namespace Tgs.SmsService.ApplicationServices;

internal class AddBrandSmsCommandService(ITgsRepository repository) : ICommandService<AddBrandSmsCommand>
{
    public async ValueTask ExecuteAsync(AddBrandSmsCommand command, CancellationToken cancellationToken = default)
        => await repository
            .AddBrandSmsAsync(command.BrandId, command.SmsSettingId, cancellationToken)
            .ConfigureAwait(false);
}