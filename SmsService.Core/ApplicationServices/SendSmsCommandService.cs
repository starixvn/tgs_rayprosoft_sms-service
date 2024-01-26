using Microsoft.Extensions.Logging;
using Tgs.ServiceUtility;

namespace Tgs.SmsService.ApplicationServices;

internal class SendSmsCommandService(ISmsSender smsSender, ILogger<SendSmsCommandService> logger) : ICommandService<SendSmsCommand>
{
    private readonly ISmsSender _smsSender = smsSender ?? throw new ArgumentNullException(nameof(smsSender));
    private readonly ILogger<SendSmsCommandService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async ValueTask ExecuteAsync(SendSmsCommand command, CancellationToken cancellationToken = default)
    {
        await _smsSender.SendSmsAsync(
            command.VendorCode.ToLower(),
            command.ShortMessage,
            cancellationToken).ConfigureAwait(false);

        _logger.Log(
            LogLevel.Information,
            new EventId(10000, "SmsSendLog"),
            new
            {
                command.VendorCode,
                command.ShortMessage.Destination,
                command.ShortMessage.Message,
                command.ShortMessage.AreaCode,
                command.ShortMessage.Phone
            },
            null,
            (state, ex) => $"[{nameof(state.VendorCode)}]{state.VendorCode}, "
                + $"[{nameof(state.Destination)}]{state.Destination}, "
                + $"[{nameof(state.Message)}]{state.Message}");
    }
}
