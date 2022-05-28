global using Saltimer.Api.Services;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql;
using Saltimer.Api.Hubs;
using Saltimer.Api.Middleware;
using Saltimer.Api.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);
{
    var connectionString = string.Empty;
    if (builder.Environment.EnvironmentName == "production")
    {
        var conStrBuilder = new NpgsqlConnectionStringBuilder();

        conStrBuilder.Host = Environment.GetEnvironmentVariable("DbHost");
        conStrBuilder.Database = Environment.GetEnvironmentVariable("DbName");
        conStrBuilder.Username = Environment.GetEnvironmentVariable("DbUsername");
        conStrBuilder.Password = Environment.GetEnvironmentVariable("DbPassword");
        conStrBuilder.SslMode = SslMode.Require;
        conStrBuilder.TrustServerCertificate = true;

        connectionString = conStrBuilder.ConnectionString;
    }
    else
    {
        connectionString = builder.Configuration.GetConnectionString("SaltimerDBContext");
    }

    builder.Services.AddDbContext<SaltimerDBContext>(
        options => options.UseNpgsql(connectionString)
    );
}


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

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddSwaggerDocument();


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
