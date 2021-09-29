using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Tgs.WebApi.Toolkits
{
    public class RetouchResponseFilter : IAsyncResultFilter
    {
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
                return;

            var successStatusCodeAttr = context.ActionDescriptor.EndpointMetadata.OfType<SuccessStatusCodeAttribute>().LastOrDefault();
            var failResponseFeature = context.HttpContext.Features.Get<IFailResponseFeature>();
            var responseFeature = context.HttpContext.Features.Get<IHttpResponseFeature>();

            if (failResponseFeature != null)
            {
                responseFeature.StatusCode = failResponseFeature.StatusCode;

                context.Result = failResponseFeature.GetActionResult();
            }
            else if (successStatusCodeAttr != null)
            {
                responseFeature.StatusCode = successStatusCodeAttr.StatusCode;
            }

            await next();
        }
    }
}
