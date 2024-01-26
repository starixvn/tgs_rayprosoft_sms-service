using Microsoft.AspNetCore.Mvc;
using Tgs.ServiceUtility;
using Tgs.SmsService;
using Tgs.SmsService.ApplicationServices;
using Tgs.WebApi.Toolkits;
using TGS.SmsService.Metrics;
using TGS.SmsService.ViewModels;

namespace TGS.SmsService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SmsController : ControllerBase
{
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
            await this.FailAsync(
                StatusCodes.Status406NotAcceptable,
                "This brand can't send sms.").ConfigureAwait(false);

            return;
        }

        var command = new SendSmsCommand(
            new ShortMessageData(
                shortMessage.BrandId,
                shortMessage.Destination,
                shortMessage.Message,
                shortMessage.AreaCode,
                shortMessage.Phone),
            brandInfo.SmsVendorCode!,
            brandInfo.SmsVendorSettings!);
        await sendSmsCommand.ExecuteAsync(
            command,
            HttpContext.RequestAborted).ConfigureAwait(false);
    }
}
