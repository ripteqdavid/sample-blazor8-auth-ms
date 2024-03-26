


using System.Reflection;

namespace BlazorAppMSAuth.Endpoints;

public static class Api
{
    public static void ConfigurePingApi(this WebApplication app)
    {
        // 1a. Ping
        var groupSahCommonPing = app.MapGroup("/api");

        groupSahCommonPing.MapGet("ping", BasicPing) //.RequireAuthorization();
           .RequireAuthorization(policy => policy.RequireRole("Administrator"));

    }

    private static async Task<IResult> BasicPing(IConfiguration _config)
    {
        try
        {
            List<string> result = new List<string>();
            Assembly assembly = Assembly.GetEntryAssembly();

            result.Add($"Environment: {Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? ""}");
            result.Add($"Machine Name: {Environment.MachineName}");
            result.Add($"OS Version: {Environment.OSVersion.ToString() ?? ""}");

            result.Add($"OS Version: {Environment.OSVersion.ToString() ?? ""}");
            result.Add($"Name: {assembly.GetName().Name ?? ""}");
            result.Add($"Version: {assembly.GetName().Version.ToString() ?? ""}");
            result.Add($".Net Framework: {AppDomain.CurrentDomain.SetupInformation.TargetFrameworkName ?? ""}");

            return Results.Ok(result);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }
}
