using Microsoft.AspNetCore.Mvc;

namespace Tgs.WebApi.Toolkits
{
    public interface IFailResponseFeature
    {
        int StatusCode { get; }

        IActionResult GetActionResult();
    }
}
