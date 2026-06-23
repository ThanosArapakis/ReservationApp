using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ReservationApp.core.api.Application;
using ReservationApp.core.api.Infrastructure;
using ReservationApp.core.api.Infrastructure.Persistence;

namespace ReservationApp.core.api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services
                .AddApplication()           
                .AddInfrastructure(builder.Configuration)
                .AddPresentation();           

            var app = builder.Build();

            // On the local Docker stack the SQL Server container starts empty, so apply
            // migrations on startup (creating the database too). Retried because the
            // container may still be warming up on the very first run.
            //if (app.Environment.IsDevelopment())
            //{
            //    using var scope = app.Services.CreateScope();
            //    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            //    for (var attempt = 1; ; attempt++)
            //    {
            //        try
            //        {
            //            db.Database.Migrate();
            //            break;
            //        }
            //        catch when (attempt < 10)
            //        {
            //            Thread.Sleep(TimeSpan.FromSeconds(5));
            //        }
            //    }
            //}

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
