using System;
using System.Net;

namespace Tgs.WebApi.Toolkits
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class FailStatusCodeAttribute : Attribute
    {
        public string? Description { get; }

        public Type? ErrorResponseType { get; }

        public int StatusCode { get; }

        public FailStatusCodeAttribute(int statusCode, string? description = null, Type? errorResponseType = null)
        {
            StatusCode = statusCode;
            Description = description;
            ErrorResponseType = errorResponseType;
        }

        public FailStatusCodeAttribute(HttpStatusCode statusCode, string? description = null, Type? errorResponseType = null)
            : this((int)statusCode, description, errorResponseType)
        {
        }
    }
}
