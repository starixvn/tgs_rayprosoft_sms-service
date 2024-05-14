using Microsoft.AspNetCore.Mvc;
using Tgs.ServiceUtility;
using Tgs.SmsService;
using Tgs.SmsService.ApplicationServices;
using TGS.SmsService.ViewModels;

namespace TGS.SmsService.Controllers;

[Route("api/sms-setting")]
[ApiController]
public class SmsSettingController : ControllerBase
{
    [HttpPost(Name = "AddSmsSetting")]
    public ValueTask AddSmsSettingAsync(
        [FromServices] ICommandService<AddSmsSettingCommand> commandService,
        [FromHeader(Name = "Modifier")] string modifier,
        [FromBody] SmsSettingRequest request)
    {
        var command = new AddSmsSettingCommand(
            request.Name,
            request.Code,
            request.Setting,
            modifier);

        return commandService.ExecuteAsync(command, HttpContext.RequestAborted);
    }

    [HttpPut("{smsSettingId:Guid}", Name = "UpdateSmsSetting")]
    public ValueTask UpdateSmsSettingAsync(
        [FromServices] ICommandService<UpdateSmsSettingCommand> commandService,
        [FromHeader(Name = "Modifier")] string modifier,
        Guid smsSettingId,
        [FromBody] SmsSettingRequest request)
    {
        var command = new UpdateSmsSettingCommand(
            smsSettingId,
            request.Name,
            request.Code,
            request.Setting,
            modifier);

        return (commandService.ExecuteAsync(command, HttpContext.RequestAborted));
    }

    [HttpDelete("{smsSettingId:Guid}", Name = "RemoveSmsSetting")]
    public ValueTask RemoveSmsSettingAsync(
        [FromServices] ICommandService<RemoveSmsSettingCommand> commandService,
        Guid smsSettingId)
        => commandService.ExecuteAsync(new RemoveSmsSettingCommand(smsSettingId), HttpContext.RequestAborted);

    [HttpGet("list", Name = "GetSmsSettings")]
    public IAsyncEnumerable<SmsSettingViewModel> GetSmsSettingsAsync(
        [FromServices] IQueryService<GetSmsSettingsQuery, SmsSettingInfo> queryService)
    {
        var query = new GetSmsSettingsQuery();

        return queryService
            .QueryAsync(query, HttpContext.RequestAborted)
            .Select(x => new SmsSettingViewModel
            {
                SmsSettingId = x.SmsSettingId,
                Name = x.Name,
                Code = x.Code,
                Setting = x.Setting,
            });
    }
}