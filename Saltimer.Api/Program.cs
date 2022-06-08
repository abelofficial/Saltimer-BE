using System.Text;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Saltimer.Api.Config;
using Saltimer.Api.Hubs;
using Saltimer.Api.Middleware;
using Saltimer.Api.Models;
using Saltimer.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
        {
            builder.WithOrigins("http://localhost:3000", "https://saltimer.vercel.app")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddSingleton<IDictionary<string, SessionHubUsers>>(
    opts => new Dictionary<string, SessionHubUsers>());
builder.Services.AddSingleton<IDictionary<string, SessionHub>>(
    opts => new Dictionary<string, SessionHub>());


builder.Services.InstallServicesFromAssembly(builder.Configuration);

var app = builder.Build();

app.UseOpenApi();
app.UseSwaggerUi3();
app.UseReDoc(c =>
{
    c.DocumentTitle = "Saltimer API";
    c.SpecUrl = "/swagger/v1/swagger.json";
});

app.UseRouting();

app.UseCors();
app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<AuthUserMiddleware>();

app.MapControllers();
app.MapHub<MobTimerHub>("/timer");


app.Run();
