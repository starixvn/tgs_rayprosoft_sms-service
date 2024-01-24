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
            command.BrandId,
            command.VendorCode.ToLower(),
            command.Destination,
            command.Message).ConfigureAwait(false);

        _logger.Log(
            LogLevel.Information,
            new EventId(10000, "SmsSendLog"),
            new
            {
                command.VendorCode,
                command.Destination,
                command.Message
            },
            null,
            (state, ex) => $"[{nameof(command.VendorCode)}]{command.VendorCode}, " +
                $"[{nameof(command.Destination)}]{command.Destination}, " +
                $"[{nameof(command.Message)}]{command.Message}");
    }
}
