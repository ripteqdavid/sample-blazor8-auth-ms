# Sample Blazor Application with Authentication

## Overview
This sample has been cobble together using a raft of resources.  The premise is to demonstrate a basic Blazor .Net 8 application that:
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
