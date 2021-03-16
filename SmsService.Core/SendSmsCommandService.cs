using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TGS.ServiceUtility;
using TGS.SmsSender;

namespace TGS.SmsService.Core
{
    internal class SendSmsCommandService : ICommandService<SmsContent>
    {
        private readonly ISmsSender _smsSender;
        private readonly ILogger<SendSmsCommandService> _logger;

        public SendSmsCommandService(ISmsSender smsSender, ILogger<SendSmsCommandService> logger)
        {
            _smsSender = smsSender ?? throw new ArgumentNullException(nameof(smsSender));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async ValueTask ExecuteAsync(SmsContent command)
        {
            await _smsSender.SendSmsAsync(
                command.BrandId,
                command.Currency,
                command.Destination,
                command.Message).ConfigureAwait(false);

            _logger.Log(
                LogLevel.Information,
                new EventId(10000, "SmsSendLog"),
                new
                {
                    command.BrandId,
                    command.Currency,
                    command.Destination,
                    command.Message
                },
                null,
                (state, ex) => $"[{nameof(command.BrandId)}]{command.BrandId}, " +
                    $"[{nameof(command.Currency)}]{command.Currency}, " +
                    $"[{nameof(command.Destination)}]{command.Destination}, " +
                    $"[{nameof(command.Message)}]{command.Message}");
        }
    }
}
