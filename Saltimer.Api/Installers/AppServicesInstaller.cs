using System.Text;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Saltimer.Api.Models;
using Saltimer.Api.Services;

namespace Saltimer.Api.Config;


public class AppServicesInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration config)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddControllers();
        services.AddSignalR();
        services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
        services.AddHttpContextAccessor();
        services.AddEndpointsApiExplorer();
        services.AddHttpContextAccessor();

        // Scopes
        services.AddScoped<IAuthService, AuthService>();
        services.AddSingleton<IDictionary<string, SessionHubUsers>>(
            opts => new Dictionary<string, SessionHubUsers>()
        );
        services.AddSingleton<IDictionary<string, SessionHub>>(
            opts => new Dictionary<string, SessionHub>()
        );

        // policies
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("http://localhost:3000", "https://saltimer.vercel.app")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
        });

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                    .GetBytes(config.GetSection("AppSettings:Token").Value)),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });
    }
}