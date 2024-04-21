using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

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
                // Redirect to login page
                context.Response.StatusCode = StatusCodes.Status302Found; // HTTP 302 for redirection
                context.Response.Headers.Location = "/Account/Login"; // Redirect to the login page
                return; // Prevent further processing after setting the redirect
            }
        }

        // If authorization is successful or response has already started, just continue the pipeline
        await next(context);
    }
}
