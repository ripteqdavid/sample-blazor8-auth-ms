using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;

namespace BlazorAppMSAuth.Services;
public class BlazorAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
{
    public async Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
    {
        if (!context.Response.HasStarted)
        {
            if (authorizeResult.Forbidden)
            {
                // Log, set status code, and write response if the response hasn't started
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Access Denied: You do not have the necessary permissions to access this resource.");
                return; // Important to return after handling the response to prevent further processing
            }

            if (authorizeResult.Challenged)
            {
                // Trigger an authentication challenge
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized: You need to authenticate to access this resource.");
                return; // Prevent further processing after handling the challenge
            }
        }

        // If authorization is successful or response has already started, just continue the pipeline
        await next(context);
    }
}
