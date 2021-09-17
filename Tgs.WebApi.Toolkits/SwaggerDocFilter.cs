using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Tgs.WebApi.Toolkits
{
    public class SwaggerDocFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var successStatusCodeAttr = context
                .ApiDescription
                .ActionDescriptor
                .EndpointMetadata
                .OfType<SuccessStatusCodeAttribute>()
                .LastOrDefault();

            if (successStatusCodeAttr != null)
            {
                operation.Responses.Clear();

                var responseType = context.ApiDescription
                    .SupportedResponseTypes
                    .FirstOrDefault();

                operation.Responses.Add(
                    successStatusCodeAttr.StatusCode.ToString(),
                    new OpenApiResponse
                    {
                        Description = successStatusCodeAttr.Description,
                        Content = responseType == null || responseType.Type == typeof(void)
                            ? null
                            : new Dictionary<string, OpenApiMediaType>
                            {
                                ["application/json"] = new OpenApiMediaType
                                {
                                    Schema = context.SchemaGenerator.GenerateSchema(
                                        responseType.Type,
                                        context.SchemaRepository)
                                }
                            }
                    });
            }

            foreach (var attr in context.ApiDescription.ActionDescriptor.EndpointMetadata.OfType<FailStatusCodeAttribute>())
            {
                operation.Responses.Add(
                    attr.StatusCode.ToString(),
                    new OpenApiResponse
                    {
                        Description = attr.Description,
                        Content = attr.ErrorResponseType == null
                            ? null
                            : new Dictionary<string, OpenApiMediaType>
                            {
                                ["application/json"] = new OpenApiMediaType
                                {
                                    Schema = context.SchemaGenerator.GenerateSchema(attr.ErrorResponseType, context.SchemaRepository)
                                }
                            }
                    });
            }
        }
    }
}
