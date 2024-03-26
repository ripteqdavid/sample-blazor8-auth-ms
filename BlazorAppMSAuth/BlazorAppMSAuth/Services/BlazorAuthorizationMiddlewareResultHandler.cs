using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace BlazorAppMSAuth.Services;
public class BlazorAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
{
    public async Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
    {
        //return next(context);

        //// Check if the authorization was successful
        if (authorizeResult.Forbidden)
        {
            // If authorization failed, determine the appropriate response
            // Here, we're setting the status code to 403 Forbidden
            // You can customize this part to log the failure, redirect the user, etc.
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync("Access Denied: You do not have the necessary role to access this resource.");


        }
        else
        {
            // If authorization is successful, continue the execution of the middleware pipeline
            await next(context);
        }
    }
}