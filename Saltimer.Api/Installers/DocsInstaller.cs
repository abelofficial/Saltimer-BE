using System.Reflection;
using NSwag;
using Swashbuckle.AspNetCore.Filters;

namespace Saltimer.Api.Config;


public class DocsInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration config)
    {
        services.AddSwaggerGen(options =>
        {
            options.OperationFilter<SecurityRequirementsOperationFilter>();
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });

        services.AddSwaggerDocument(config =>
        {
            config.Version = "v1";
            config.Title = "Saltimer";
            config.Description = "A RestAPI for Saltimer.";
            config.AddSecurity("oauth2", new NSwag.OpenApiSecurityScheme
            {
                Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
                In = OpenApiSecurityApiKeyLocation.Header,
                Name = "Authorization",
                Type = OpenApiSecuritySchemeType.ApiKey
            });
        });
    }
}