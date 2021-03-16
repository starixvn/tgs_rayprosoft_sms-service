using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TGS.ServiceUtility;
using TGS.SmsService.Core;
using TGS.SmsService.Metrics;
using TGS.SmsService.ViewModels;

namespace TGS.SmsService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SmsController : ControllerBase
    {
        private readonly ILogger<SmsController> _logger;

        public SmsController(ILogger<SmsController> logger)
        {
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        /// <summary>Enqueue the short message to send.</summary>
        /// <param name="sendSmsCommand">The send SMS command.</param>
        /// <param name="shortMessage">The short message.</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [SuccessStatusCode(StatusCodes.Status202Accepted)]
        [PerformanceCounter(typeof(SmsApiMeter), nameof(SmsApiMeter.IncrementSendSmsRequest))]
        public async Task SendSmsAsync([FromServices] ICommandService<SmsContent> sendSmsCommand, [FromBody] SmsViewModel shortMessage)
        {
            await sendSmsCommand.ExecuteAsync(
                new SmsContent(
                    shortMessage.Destination,
                    shortMessage.Message,
                    shortMessage.Currency,
                    shortMessage.BrandId)).ConfigureAwait(false);

            //var client = new Client(new Credentials("112b1a8ac175bab43037e2e4e4542c69", "2c7aa3b02677703b58d8543c43ceacca"));

            //var response = await client.SMS.SMSSend(new
            //{
            //    origin = "Test",
            //    destination = smsContent.Destination,
            //    message = smsContent.Message
            //});
        }
    }
}
