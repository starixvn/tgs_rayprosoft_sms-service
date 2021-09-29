using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Features;
using Tgs.WebApi.Toolkits;

namespace Microsoft.AspNetCore.Mvc
{
    public static class ApiControllerExtensions
    {
        public static TResult Fail<TResult>(this ControllerBase controller, HttpStatusCode statusCode, string errorMessage)
        {
            return Fail<TResult>(controller, (int)statusCode, errorMessage);
        }

        public static TResult Fail<TResult>(this ControllerBase controller, int statusCode, string errorMessage)
        {
            Fail(controller, statusCode, errorMessage);

            return default!;
        }

        public static Task<TResult> FailAsync<TResult>(this ControllerBase controller, HttpStatusCode statusCode, string errorMessage)
            => FailAsync<TResult>(controller, (int)statusCode, errorMessage);

        public static Task<TResult> FailAsync<TResult>(this ControllerBase controller, int statusCode, string errorMessage)
            => Task.FromResult(Fail<TResult>(controller, statusCode, errorMessage));

        public static void Fail(this ControllerBase controller, HttpStatusCode statusCode, string errorMessage)
            => Fail(controller, (int)statusCode, errorMessage);

        public static void Fail(this ControllerBase controller, int statusCode, string errorMessage)
        {
            var responseFeature = controller.HttpContext.Features.Get<IHttpResponseFeature>();

            responseFeature.StatusCode = statusCode;

            var failResponseFeature = new FailResponseFeature<string>((int)statusCode, errorMessage);

            controller.HttpContext.Features.Set<IFailResponseFeature>(failResponseFeature);
        }

        public static Task FailAsync(this ControllerBase controller, HttpStatusCode statusCode, string errorMessage)
            => FailAsync(controller, (int)statusCode, errorMessage);

        public static Task FailAsync(this ControllerBase controller, int statusCode, string errorMessage)
        {
            Fail(controller, statusCode, errorMessage);

            return Task.CompletedTask;
        }
    }
}
