using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Tgs.SmsService;
using Tgs.SmsService.ApplicationServices;
using Tgs.WebApi.Toolkits;
using TGS.ServiceUtility;
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
        [SuccessStatusCode(StatusCodes.Status202Accepted, "Request queued.")]
        [FailStatusCode(StatusCodes.Status406NotAcceptable, "Request rejected.")]
        [PerformanceCounter(typeof(SmsApiMeter), nameof(SmsApiMeter.IncrementSendSmsRequest))]
        public async Task SendSmsAsync(
            [FromServices] IGetService<GetBrandInfoQuery, BrandInfo?> getBrandService,
            [FromServices] ICommandService<SendSmsCommand> sendSmsCommand,
            [FromBody] SmsViewModel shortMessage)
        {
            var brandQuery = new GetBrandInfoQuery(shortMessage.BrandId);

            var brandInfo = await getBrandService
                .GetAsync(brandQuery, HttpContext.RequestAborted)
                .ConfigureAwait(false);

            if (brandInfo?.SmsEnabled != true)
            {
                await this.Fail<Task>(
                    StatusCodes.Status406NotAcceptable,
                    "This brand can't send sms.").ConfigureAwait(false);

                return;
            }

            await sendSmsCommand.ExecuteAsync(
                new SendSmsCommand(
                    shortMessage.Destination,
                    shortMessage.Message,
                    brandInfo.SmsVendorCode!,
                    brandInfo.SmsVendorSettings!)).ConfigureAwait(false);
        }
    }
}
