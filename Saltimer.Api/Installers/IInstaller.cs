namespace Saltimer.Api.Config;


public interface IInstaller
{
    void InstallServices(IServiceCollection services, IConfiguration config);
}