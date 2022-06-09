using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Saltimer.Api.Config;


public class DbServicesInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration config)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var isProduction = environment == Environments.Production;

        var connectionString = string.Empty;
        if (isProduction)
        {
            var conStrBuilder = new NpgsqlConnectionStringBuilder();

            conStrBuilder.Host = Environment.GetEnvironmentVariable("DbHost");
            conStrBuilder.Database = Environment.GetEnvironmentVariable("DbName");
            conStrBuilder.Username = Environment.GetEnvironmentVariable("DbUsername");
            conStrBuilder.Password = Environment.GetEnvironmentVariable("DbPassword");
            conStrBuilder.SslMode = SslMode.Require;
            conStrBuilder.TrustServerCertificate = true;

            connectionString = conStrBuilder.ConnectionString;
            services.AddDbContext<SaltimerDBContext>(
                options => options.UseNpgsql(connectionString)
       );
        }
        else
        {
            connectionString = config.GetConnectionString("SaltimerDBContext");
            services.AddDbContext<SaltimerDBContext>(options =>
                  options.UseSqlServer(connectionString ?? throw new InvalidOperationException("Connection string 'SaltimerDBContext' not found.")));
        }
    }
}