using Microsoft.AspNetCore.Mvc.Filters;

namespace TGS.SmsService
{
    public class SuccessStatusCodeAttribute : ActionFilterAttribute
    {
        public int StatusCode { get; }

        public SuccessStatusCodeAttribute(int statusCode)
        {
            StatusCode = statusCode;
        }

        public override void OnResultExecuted(ResultExecutedContext context)
        {
            if (context.ModelState.IsValid && context.Exception == null)
                context.HttpContext.Response.StatusCode = StatusCode;
        }
    }
}
