using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PlatformService.Models;
using System;
using System.Linq;

namespace PlatformService.Data
{
    public static class PrepDb
    {

        public static void PrepPoulation(IApplicationBuilder app, bool IsProd)
        {
            using (var servicescope = app.ApplicationServices.CreateScope())
            {
                SeedData(servicescope.ServiceProvider.GetService<AppDbContext>(), IsProd);
            }
        }

        private static void SeedData(AppDbContext dbContext, bool IsProd)
        {
            if (IsProd)
            {
                System.Console.WriteLine("We are applying migrations");
                try
                {
                    dbContext.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not run migrate: {ex.Message}");
                }
            }
            else
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
}
