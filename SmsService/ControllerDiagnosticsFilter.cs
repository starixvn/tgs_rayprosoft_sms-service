using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TGS.SmsService
{
    public class ControllerDiagnosticsFilter : IAsyncActionFilter
    {
        private static readonly ActivitySource _ActivitySource = new ActivitySource("TGS.SmsService.Controllers");

        public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            using var activity = _ActivitySource.StartActivity(
                context.ActionDescriptor.DisplayName
                    ?? $"{context.Controller.GetType().FullName}.{context.ActionDescriptor.RouteValues["action"]}");

            var counters = context.Filters.OfType<PerformanceCounterAttribute>().ToArray();

            if (counters.Length > 0)
            {
                Action counterAction = () =>
                {
                    for (var i = 0; i < counters.Length; i++)
                        counters[i].CountAction.Invoke();
                };
                activity?.SetCustomProperty("PerformanceCounter", counterAction);
            }

            return next();
        }
    }
}
