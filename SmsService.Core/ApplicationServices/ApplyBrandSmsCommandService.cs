using Tgs.ServiceUtility;

namespace Tgs.SmsService.ApplicationServices;

internal class ApplyBrandSmsCommandService(ITgsRepository repository)
    : ICommandService<ApplyBrandSmsCommand>
{
    public async ValueTask ExecuteAsync(ApplyBrandSmsCommand command, CancellationToken cancellationToken = default)
    {
        var smsSetting = await repository
            .GetSmsSettingByIdAsync(command.SmsSettingId, cancellationToken)
            .ConfigureAwait(false);

        if (smsSetting is null)
            return;

        await repository.ApplyBrandSmsAsync(
            command.BrandId,
            smsSetting.Code,
            smsSetting.Setting,
            command.Modifier,
            cancellationToken).ConfigureAwait(false);
    }
}