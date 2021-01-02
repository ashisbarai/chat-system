using System;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Threading.Tasks;
using Chat.Api.Core.Configs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Chat.Api.Web.Middlewares
{
    public class ApiKeyMiddleware
    {
        public ApiKeyMiddleware(RequestDelegate next, ApiKeyConfig apiKeyConfig)
        {
            _next = next;
            _apiKeyConfig = apiKeyConfig;
        }
        private readonly RequestDelegate _next;
        private readonly ApiKeyConfig _apiKeyConfig;

        public async Task Invoke(HttpContext context, ILogger<ApiKeyMiddleware> logger)
        {
            bool ContainsHeader(string name)
            {
                return context.Request.Headers.Keys.Contains(name, StringComparer.InvariantCultureIgnoreCase);
            }
            if (ContainsHeader("x-api-key") && ContainsHeader("x-app-id"))
            {
                // validate the supplied API key
                // Validate it
                var headerKey = context.Request.Headers["x-api-key"].FirstOrDefault();
                var appId = context.Request.Headers["x-app-id"].FirstOrDefault();

                var apiKey = new ApiKey
                {
                    AppId = appId,
                    Key = headerKey
                };

                logger.LogInformation("API access request with {@ApiKey}", apiKey);

                await ValidateApiKey(context, _next, logger, apiKey);
            }
            else
            {
                await _next.Invoke(context);
            }
        }
        private async Task ValidateApiKey(HttpContext context, RequestDelegate next, ILogger<ApiKeyMiddleware> logger, ApiKey apiKey)
        {
            var isValidKey = _apiKeyConfig.ApiKeys.Any(k=>k.AppId == apiKey.AppId && k.Key == apiKey.Key);
            // validate it here
            //var valid = key == "api-key";
            if (!isValidKey)
            {
                logger.LogWarning("Invalid API Key Or App Id {@ApiKey}", apiKey);
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Invalid API Key Or App Id");
            }
            else
            {
                var identity = new GenericIdentity("API");
                var principal = new GenericPrincipal(identity, new[] { "Admin", "ApiUser" });
                context.User = principal;
                await next.Invoke(context);
            }
        }
    }
}