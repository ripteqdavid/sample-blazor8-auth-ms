using Microsoft.OpenApi.Models;
using System.Reflection;

namespace BlazorAppMSAuth.Configuration
{
    public static class StartupConfig
    {
        public static void AddStandardServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddEndpointsApiExplorer();

            builder.AddSwaggerServices();
        }

        public static void AddSwaggerServices(this WebApplicationBuilder builder)
        {
            var securityScheme = new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Description = "JWT Authorization header using the Bearer scheme.",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT"
            };

            var securityRequirement = new OpenApiSecurityRequirement
{
    {
    new OpenApiSecurityScheme
    {
        Reference = new OpenApiReference
        {
            Id = "bearerAuth",
            Type = ReferenceType.SecurityScheme
        }
    },
    new string[] { }
    }
};


            builder.Services.AddSwaggerGen(opts =>
            {
                opts.AddSecurityDefinition("bearerAuth", securityScheme);
                opts.AddSecurityRequirement(securityRequirement);
                opts.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
                {
                    Description = "Api Key Authentication",
                    Type = SecuritySchemeType.ApiKey,
                    Name = "Token",
                    In = ParameterLocation.Header,
                    Scheme = "ApiKeyScheme"
                });

                var scheme = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Id = "ApiKey",
                        Type = ReferenceType.SecurityScheme
                    },
                    In = ParameterLocation.Header
                };

                var requirement = new OpenApiSecurityRequirement
                {
    { scheme, new List<string>() }
                };

                opts.AddSecurityRequirement(requirement);

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //opts.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

                opts.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "TalkBack Api",
                    Description = "Talk Back API"
                });
            });
        }
    }
}
