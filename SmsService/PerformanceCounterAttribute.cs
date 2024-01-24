using Microsoft.AspNetCore.Mvc.Filters;

namespace TGS.SmsService;

public class PerformanceCounterAttribute(Type meterType, string methodName) : ActionFilterAttribute
{
    internal Action CountAction { get; } = meterType.GetMethod(methodName)!.CreateDelegate<Action>();
}
