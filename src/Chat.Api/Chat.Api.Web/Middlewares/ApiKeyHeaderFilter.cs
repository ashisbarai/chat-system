using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Chat.Api.Web.Middlewares
{
    public class ApiKeyHeaderFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "x-app-id",
                In = ParameterLocation.Header,
                Required = false,
                Example = new OpenApiString("")
            });
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "x-api-key",
                In = ParameterLocation.Header,
                Required = false,
                Example = new OpenApiString("")
            });
        }
    }
}
