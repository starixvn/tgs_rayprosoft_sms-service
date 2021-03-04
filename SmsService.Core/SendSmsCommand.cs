using System;
using System.Threading.Tasks;
using TGS.Sms;
using Valhalla.Services;

namespace TGS.SmsService
{
    internal class SendSmsCommand : ICommandService<SmsContent>
    {
        private readonly ISmsSender _smsSender;

        public SendSmsCommand(ISmsSender smsSender)
        {
            _smsSender = smsSender ?? throw new ArgumentNullException(nameof(smsSender));
        }

        public async ValueTask ExecuteAsync(SmsContent command)
        {
            await _smsSender.SendSmsAsync(
                command.Brand,
                command.Currency,
                command.Destination,
                command.Message).ConfigureAwait(false);
        }
    }
}
