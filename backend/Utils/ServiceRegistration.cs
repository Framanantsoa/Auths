using System.Reflection;

namespace Utils;

public static class ServiceRegistration
{
    public static void AddProjectServices(this IServiceCollection services) {
        // Recherche toutes les classes dans le namespace "Services"
        var serviceTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.Namespace != null && t.Namespace.StartsWith("Services") 
                && t.IsClass && !t.IsAbstract);

        foreach (var type in serviceTypes) {
            var interfaceType = type.GetInterfaces().FirstOrDefault();
            
            if (interfaceType != null) {
                services.AddScoped(interfaceType, type);
            }
            else services.AddScoped(type);
        }
    }
}
