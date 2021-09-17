using System;
using System.Net;

namespace Tgs.WebApi.Toolkits
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class SuccessStatusCodeAttribute : Attribute
    {
        public string? Description { get; }

        public int StatusCode { get; }

        public SuccessStatusCodeAttribute(int statusCode, string? description = null)
        {
            StatusCode = statusCode;
            Description = description;
        }

        public SuccessStatusCodeAttribute(HttpStatusCode statusCode, string? description = null)
            : this((int)statusCode, description)
        {
        }
    }
}
