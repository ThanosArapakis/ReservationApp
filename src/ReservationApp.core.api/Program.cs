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

            //builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services
                .AddApplication()           
                .AddInfrastructure(builder.Configuration)
                .AddPresentation();           

            var app = builder.Build();

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
