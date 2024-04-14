# Sample Blazor .Net 8 App Application with Authentication

## Introduction
This sample has been cobble together using a raft of resources.  The premise is to demonstrate a basic Blazor .Net 8 application that:
- Uses both Server and Client pages
- Uses the Microsoft Identity provider
- Implements roles
- Includes database migrations as part of the program.cs
- Implements Serilog for logging (and Raygun)
- Implements Swagger
- Utilises the same security process in both the Server, and WASM Client
- Implements minimal API endpoints with role based authentication
- Shows how the client can create an HTTP Client Factory (Named) and call a Server endpoint.

## Resources
- https://jonhilton.net/blazor-auth0-net8/
- https://auth0.com/blog/auth0-authentication-blazor-web-apps/
- https://github.com/auth0-blog/blazor-interactive-auto
- https://stackoverflow.com/questions/77865308/how-to-actually-implement-roles-in-blazor-webapp-and-identity
- https://www.youtube.com/watch?v=rCnsJwMc6-I
- https://learn.microsoft.com/en-us/aspnet/core/blazor/call-web-api?view=aspnetcore-8.0
- Patrick God Youtube https://www.youtube.com/@PatrickGod
- https://stackoverflow.com/questions/77624318/blazor-net-8-web-api-in-blazor-server-project-is-not-working?noredirect=1&lq=1
- https://github.com/CrahunGit/Auth0BlazorWebAppSample


## Introduction
When you begin a 'File > New' project, there are still quite a few setup options to undertake before you have the bones of a workable solution.  This demo project shows how some of these modules could be set up.

## What Modules?
- Swagger
- Serilog
- Role based authentication and authorization within Blazor
- [Users local date/time](https://www.meziantou.net/convert-datetime-to-user-s-time-zone-with-server-side-blazor-time-provider.htm)
- Minimal API endpoints that use Blazor login authentication (internal use)
- Role based authentication

### Swagger
- When you start the application, you can browse to:
	- https://localhost:7076/swagger
- Nuget Packages
	- Swashbuckle.AspNetCore.SwaggerGen
	- Swashbuckle.AspNetCore.SwaggerUI
- Setup as a static extensions method
	- Configuration > StartupConfig.cs
	- Program.cs 
```
builder.AddStandardServices();
```
- Enable UI for Developer settings in Program.cs
```
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseMigrationsEndPoint();

    // Include Swagger if Development
    app.UseSwagger(); // <---
    app.UseSwaggerUI(); // <---
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
```

### Serilog
- Nuget:
	- Serilog.AspNetCore
	- Serilog.Enrichers.Environment
	- Serilog.Enrichers.Process
	- Serilog.Enrichers.Thread
	- Serilog.Settings.Configuration
	- Serilog.Sinks.MSSqlServer
	- Serilog.Sinks.Raygun
- Serilog is pretty handy debugging Blazor.  Often errors just get swallowed by the application with no visibility to the issue.  
- By implementing Serilog on the console, this gives you a better development experience
- Included is configuration for Console, SQL Server an RayGun (appsettings), however you can change this as you see fit.

```
 "Serilog": {
     "Using": [ "Serilog.Sinks.Console", "Serilog.Sinks.MSSqlServer", "Serilog.Sinks.Raygun" ],
     "MinimumLevel": {
       "Default": "Information",
       "Override": {
         "Microsoft": "Warning",
         "System": "Warning"
       }
     },
     "Enrich": [ "FromLogContext", "WithMachineName", "WithThreadId", "WithProcessId" ],
     "WriteTo": [
       {
         "Name": "Console",
         "Args": {
           "outputTemplate": "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
         }
       },
       {
         "Name": "MSSqlServer",
         "Args": {
           "connectionString": "Server=localhost;Initial Catalog=BlazorMSAuthTest2;Persist Security Info=False;Integrated Security=True;Trusted_Connection=SSPI;Encrypt=false;TrustServerCertificate=true",
           "tableName": "SeriLogs",
           "autoCreateSqlTable": true,
           "batchPostingLimit": 2,
           "restrictedToMinimumLevel": "Information",
           "columnOptionsSection": {
             "addTimestamp": true,
             "disableTriggers": true,
             "removeStandardColumns": [ "MessageTemplate" ],
             "primaryKeyColumnName": "Id",
             "additionalColumns": [
               {
                 "ColumnName": "MachineName",
                 "DataType": "nvarchar",
                 "DataLength": 64
               },
               {
                 "ColumnName": "ThreadId",
                 "DataType": "int"
               }
               ,
               {
                 "ColumnName": "TrackingId",
                 "DataType": "nvarchar",
                 "DataLength":  64
               }
             ]
           }
         }
       },
       {
         "Name": "Raygun",
         "Args": {
           "applicationKey": "AddYourRayGunKeyHere",
           "LogEverything": true,
           "LogEventLevel": "Error"
         }
       }
     ]
   }
```

### Authentication and Authorization
- Client
	- Nuget:  Microsoft.Extensions.Http
	- Update UserInfo.cs to add Roles
	- Update PersistentAuthenticationStateProvider.cs to add Roles to the claims object for a user
	- Add CookieHandler.cs
	- Add Services > HostingEnvironmentService.cs (not required but handy)
	- Update Program.cs (as per GitHub)
- \Server:
	- Update PersistingRevalidatingAuthenticationStateProvider.cs to include Roles
	- Update Program.cs
		- Include Database Seeding Function (Roles and Default User)
		- Will need to add a project reference to the client project (shared UserInfo.cs model)
	- Update Routes.razor
```
@using BlazorAppMSAuth.Client
<Router AppAssembly="@typeof(Program).Assembly">
    <Found Context="routeData">
      <AuthorizeRouteView RouteData="routeData" DefaultLayout="typeof(Layout.MainLayout)">
        <NotAuthorized>
          <RedirectToLogin />
        </NotAuthorized>
      </AuthorizeRouteView>
      <FocusOnNavigate RouteData="routeData" Selector="h1" />
    </Found>
</Router>
```


### Minimal API
- These have been moved to as static configuration file (Endpoints > Api.cs)
- Referenced in Program.cs
- Uses the BlazorAuthorizationMiddlewareResultHandler.cs as authentication middleware
- Surfaced to the Swagger UI for testing, and useable directly from the Blazor Client pages (see .Client > Pages > Counter)
- Authorization can occur in the Razor syntax, or applied to the API endpoints.
```
var groupAuth = app.MapGroup("/api");
var groupNoAuth = app.MapGroup("/api/noauth");

groupAuth.MapGet("ping", BasicPing)
   .RequireAuthorization(policy => policy.RequireRole("Admin"));

groupNoAuth.MapGet("pong", NoAuthPing)
    .AllowAnonymous();
```


### Server Browser Local Data and Time
- Implemented module as described in:
	- https://www.meziantou.net/convert-datetime-to-user-s-time-zone-with-server-side-blazor-time-provider.htm


