using System.Text;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using Saltimer.Api.Config;
using Saltimer.Api.Hubs;
using Saltimer.Api.Middleware;
using Saltimer.Api.Models;
using Saltimer.Api.Services;

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
        builder.Services.AddDbContext<SaltimerDBContext>(
            options => options.UseNpgsql(connectionString)
   );
    }
    else
    {
        connectionString = builder.Configuration.GetConnectionString("SaltimerDBContext");
        builder.Services.AddDbContext<SaltimerDBContext>(options =>
             options.UseSqlServer(connectionString ?? throw new InvalidOperationException("Connection string 'UserContext' not found.")));
    }


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

// builder.Services.AddSwaggerGen(options =>
// {

//     options.OperationFilter<SecurityRequirementsOperationFilter>();
//     var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
//     options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
// });

// builder.Services.AddSwaggerDocument(config =>
// {
//     config.Version = "v1";
//     config.Title = "Saltimer";
//     config.Description = "A RestAPI for Saltimer.";
//     config.AddSecurity("oauth2", new NSwag.OpenApiSecurityScheme
//     {
//         Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
//         In = OpenApiSecurityApiKeyLocation.Header,
//         Name = "Authorization",
//         Type = OpenApiSecuritySchemeType.ApiKey
//     });
// });

builder.Services.InstallServicesFromAssembly(builder.Configuration);

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
