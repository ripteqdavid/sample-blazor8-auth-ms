


using Serilog;
using System.Reflection;
using System.Security.Claims;

namespace BlazorAppMSAuth.Endpoints;

public static class Api
{
    public static void ConfigurePingApi(this WebApplication app)
    {
        // 1a. Ping
        var groupAuth = app.MapGroup("/api");
        var groupNoAuth = app.MapGroup("/api/noauth");

        groupAuth.MapGet("ping", BasicPing)
           .RequireAuthorization(policy => policy.RequireRole("Admin"));

        // ping with minimal auth (no roles, etc)
        groupAuth.MapGet("pang", DifferentPing)
            .RequireAuthorization();

        groupNoAuth.MapGet("pong", NoAuthPing)
            .AllowAnonymous();

    }

    private static IResult DifferentPing(HttpContext context)
    {
        var message = "Different Ping with Auth";
        Log.Information(message);
        return Results.Ok(message);
    }

    private static async Task<IResult> BasicPing(IConfiguration _config, HttpContext context)
    {
        try
        {
            Log.Information("Starting Basic Ping with Auth");
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            List<string> result = new List<string>();
            Assembly assembly = Assembly.GetEntryAssembly();
            result.Add("Ping: With Added Auth");
            result.Add("=====================");
            result.Add($"User: {userId}");
            result.Add($"Environment: {Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? ""}");
            result.Add($"Machine Name: {Environment.MachineName}");
            result.Add($"OS Version: {Environment.OSVersion.ToString() ?? ""}");
            result.Add($"Name: {assembly.GetName().Name ?? ""}");
            result.Add($"Version: {assembly.GetName().Version.ToString() ?? ""}");
            result.Add($".Net Framework: {AppDomain.CurrentDomain.SetupInformation.TargetFrameworkName ?? ""}");
            

            return Results.Ok(result);

        }
        catch (Exception ex)
        {
            Log.Error("Failed Basic Ping with Auth {ex}", ex);
            return Results.BadRequest(ex.Message);
        }
    }

    private static async Task<IResult> NoAuthPing()
    {
        try
        {
            Log.Information("Starting NoAuth Ping");
            List<string> result = new List<string>();
            result.Add("Ping: No Auth");
            result.Add("=============");
            result.Add($"Environment: {Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? ""}");
            result.Add($"Machine Name: {Environment.MachineName}");
            result.Add($"OS Version: {Environment.OSVersion.ToString() ?? ""}");

            return Results.Ok(result);
        }
        catch (Exception ex)
        {
            Log.Error("Failed NoAuth Ping {ex}", ex);
            return Results.BadRequest(ex.Message);
        }
    }
}
