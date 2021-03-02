using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TGS.Sms;
using TGS.SmsService.ViewModels;

namespace TGS.SmsService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SmsController : ControllerBase
    {
        [HttpPost]
        public async Task SendSmsAsync([FromServices] ISmsSender smsSender, [FromBody] SmsViewModel smsContent)
        {
            await smsSender.SendSmsAsync(smsContent.Currency, smsContent.Destination, smsContent.Message).ConfigureAwait(false);
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
