using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using PlatformService.Models;
using System.Linq;

namespace PlatformService.Data
{
    public static class PrepDb
    {

        public static void PrepPoulation(IApplicationBuilder app)
        {
            using (var servicescope = app.ApplicationServices.CreateScope())
            {
                SeedData(servicescope.ServiceProvider.GetService<AppDbContext>());
            }
        }

        private static void SeedData(AppDbContext dbContext)
        {
            if (!dbContext.Platforms.Any())
            {
                System.Console.WriteLine("Seeding Data");

                dbContext.Platforms.AddRange(
                     new Platform() { Name = "DotNet", Publisher = "Microsoft", Cost = "Free" },
                     new Platform() { Name = "Java", Publisher = "Sun Systems", Cost = "100" },
                     new Platform() { Name = "Kubernetes", Publisher = "Cloud", Cost = "100" },
                     new Platform() { Name = "Docker", Publisher = "Cloud", Cost = "Free" },
                     new Platform() { Name = "Python", Publisher = "Microsoft", Cost = "Free" });

                dbContext.SaveChanges();
            }
            else
            {
                System.Console.WriteLine("We already have data");
            }
        }
    }
}
