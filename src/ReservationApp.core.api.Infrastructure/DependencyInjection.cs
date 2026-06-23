using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReservationApp.core.api.Application.Common.Interfaces.Notifications;
using ReservationApp.core.api.Application.Common.Interfaces.Restaurant;
using ReservationApp.core.api.Infrastructure.Notifications;
using ReservationApp.core.api.Infrastructure.Notifications.Senders;
using ReservationApp.core.api.Infrastructure.Persistence;
using ReservationApp.core.api.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationApp.core.api.Infrastructure
{
    public static  class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration) =>
            services
            .AddHttpClients()
            .AddServices()
            .AddNotifications()
            .AddDatabases(configuration);

        private static IServiceCollection AddHttpClients(this IServiceCollection services)
        {
            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IRestaurantRepository, RestaurantRepository>();
            services.AddScoped<IMenuItemRepository, MenuItemRepository>();
            services.AddScoped<IReservationRepository, ReservationRepository>();
            return services;
        }

        private static IServiceCollection AddNotifications(this IServiceCollection services)
        {
            // Register one sender per channel. They are all resolved together as
            // IEnumerable<INotificationSender> by the factory — adding a channel later is
            // just another line here.
            services.AddScoped<INotificationSender, MessageBrokerNotificationSender>();
            services.AddScoped<INotificationSender, EmailNotificationSender>();

            services.AddScoped<INotificationSenderFactory, NotificationSenderFactory>();
            services.AddScoped<INotificationService, NotificationService>();
            return services;
        }

        private static IServiceCollection AddDatabases(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            return services;
        }

    }
}
