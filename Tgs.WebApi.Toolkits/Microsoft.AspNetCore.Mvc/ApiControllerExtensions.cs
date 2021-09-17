using System.Net;
using Microsoft.AspNetCore.Http.Features;
using Tgs.WebApi.Toolkits;

namespace Microsoft.AspNetCore.Mvc
{
    public static class ApiControllerExtensions
    {
        public static TResult Fail<TResult>(this ControllerBase controller, HttpStatusCode statusCode, string errorMessage)
        {
            var responseFeature = controller.HttpContext.Features.Get<IHttpResponseFeature>();

            responseFeature.StatusCode = (int)statusCode;

            var failResponseFeature = new FailResponseFeature<string>((int)statusCode, errorMessage);

            controller.HttpContext.Features.Set<IFailResponseFeature>(failResponseFeature);

            return default!;
        }

        public static TResult Fail<TResult>(this ControllerBase controller, int statusCode, string errorMessage)
        {
            var responseFeature = controller.HttpContext.Features.Get<IHttpResponseFeature>();

            responseFeature.StatusCode = statusCode;

            var failResponseFeature = new FailResponseFeature<string>((int)statusCode, errorMessage);

            controller.HttpContext.Features.Set<IFailResponseFeature>(failResponseFeature);

            return default!;
        }
    }
}
