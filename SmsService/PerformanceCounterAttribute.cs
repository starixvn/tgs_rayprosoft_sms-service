using Microsoft.AspNetCore.Mvc.Filters;

namespace TGS.SmsService;

public class PerformanceCounterAttribute : ActionFilterAttribute
{
    internal Action CountAction { get; }

    public PerformanceCounterAttribute(Type meterType, string methodName)
    {
        CountAction = meterType.GetMethod(methodName)!.CreateDelegate<Action>();
    }
}
