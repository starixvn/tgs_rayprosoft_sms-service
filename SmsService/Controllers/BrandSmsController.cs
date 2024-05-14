using Microsoft.AspNetCore.Mvc;
using Tgs.ServiceUtility;
using Tgs.SmsService;
using Tgs.SmsService.ApplicationServices;
using TGS.SmsService.ViewModels;

namespace TGS.SmsService.Controllers;

[Route("api/brand-sms")]
[ApiController]
public class BrandSmsController : ControllerBase
{
    [HttpPost("{brandId:Guid}", Name = "AddBrandSms")]
    public ValueTask AddBrandSmsAsync(
        [FromServices] ICommandService<AddBrandSmsCommand> commandService,
        Guid brandId,
        [FromBody] Guid smsSettingId)
    {
        var command = new AddBrandSmsCommand(
            brandId,
            smsSettingId);

        return commandService.ExecuteAsync(command, HttpContext.RequestAborted);
    }

    [HttpDelete("{brandId:Guid}/{smsSettingId:Guid}", Name = "RemoveBrandSms")]
    public ValueTask RemoveBrandSmsAsync(
        [FromServices] ICommandService<RemoveBrandSmsCommand> commandService,
        Guid brandId,
        Guid smsSettingId)
        => commandService.ExecuteAsync(new RemoveBrandSmsCommand(brandId, smsSettingId), HttpContext.RequestAborted);

    [HttpGet("list/{brandId:Guid}", Name = "QueryBrandSmsList")]
    public IAsyncEnumerable<BrandSmsViewModel> QueryBrandSmsListAsync(
        [FromServices] IQueryService<GetBrandSmsQuery, BrandSmsInfo> queryService,
        Guid brandId)
    {
        var query = new GetBrandSmsQuery(brandId);

        return queryService
            .QueryAsync(query, HttpContext.RequestAborted)
            .Select(x => new BrandSmsViewModel
            {
                BrandId = brandId,
                SmsSettingId = x.SmsSettingId,
            });
    }

    [HttpPatch("apply/{brandId:Guid}")]
    public ValueTask ApplyBrandSmsAsync(
        [FromServices] ICommandService<ApplyBrandSmsCommand> commandService,
        [FromHeader(Name = "Modifier")] string modifier,
        [FromBody] Guid smsSettingId,
        Guid brandId)
    {
        var command = new ApplyBrandSmsCommand(brandId, smsSettingId, modifier);

        return commandService.ExecuteAsync(command);
    }
}