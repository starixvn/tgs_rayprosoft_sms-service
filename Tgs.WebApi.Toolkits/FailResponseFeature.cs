using Microsoft.AspNetCore.Mvc;

namespace Tgs.WebApi.Toolkits
{
    public class FailResponseFeature<TResult> : IFailResponseFeature
    {
        private readonly TResult _result;

        public int StatusCode { get; }

        public FailResponseFeature(int statusCode, TResult result)
        {
            StatusCode = statusCode;
            _result = result;
        }

        public IActionResult GetActionResult()
        {
            return new ObjectResult(_result);
        }
    }
}
