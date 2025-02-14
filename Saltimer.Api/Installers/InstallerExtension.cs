using System.Reflection;
namespace Saltimer.Api.Config;


public static class InstallerExtension
{
    public static void InstallServicesFromAssembly(this IServiceCollection services, IConfiguration config)
    {
        var installers = Assembly.GetExecutingAssembly().ExportedTypes
        .Where(x =>
            typeof(IInstaller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract
        )
        .Select(Activator.CreateInstance)
        .Cast<IInstaller>()
        .ToList();

        installers.ForEach(i => i.InstallServices(services, config));
    }
}