using BlazorAppMSAuth.Services;

namespace BlazorAppMSAuth.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBrowserTimeProvider(this IServiceCollection services)
        => services.AddTransient<TimeProvider, BrowserTimeProvider>();
}
