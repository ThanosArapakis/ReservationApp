
using Microsoft.Extensions.DependencyInjection;
using OpenMediator;
using ReservationApp.core.api.Application;
using System.Reflection;

namespace ReservationApp.core.api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddOpenMediator(config =>
            config.RegisterCommandsFromAssemblies(
                Assembly.GetExecutingAssembly(),
                typeof(Application.DependencyInjection).Assembly
            ));

            return services;
        }
    }
}
