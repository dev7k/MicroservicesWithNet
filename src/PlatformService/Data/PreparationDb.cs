using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data
{
    public static class PreparationDb
    {
        public static void PreparePopulation(IApplicationBuilder app, bool IsProduction)
        {
            using(var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), IsProduction);
            }
        }

        private static void SeedData(AppDbContext context, bool IsProduction)
        {
            if (IsProduction)
            {
                Console.WriteLine("--> Attempting to apply migrations...");

                try
                {
                    context.Database.Migrate();
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"--> Could not run migrations: {exception.Message}");
                }
            }

            if(!context.Platforms.Any())
            {
                Console.WriteLine("--> Seeding Data...");

                context.Platforms.AddRange
                (
                    new Platform() {Name="Dot Net", Publisher="Microsoft", Cost="Free"},
                    new Platform() {Name="SQL Server Express", Publisher="Microsoft",  Cost="Free"},
                    new Platform() {Name="Kubernetes", Publisher="Cloud Native Computing Foundation",  Cost="Free"}
                );

                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> We already have data");
            }
        }
    }
}
