using Microsoft.EntityFrameworkCore;
using PlatformWebService.Models;
namespace PlatformWebService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app,bool isProduction)
        {
            using(var serviceScope= app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(),isProduction);
            }
        }

        private static void SeedData(AppDbContext dbContext,bool isProduction)
        {
            if(isProduction){
                Console.WriteLine("--> Attemping to apply migrations . . .");
                try{
                    dbContext.Database.Migrate();
                }
                catch(Exception ex){
                    Console.WriteLine($"--->Could not run migration:{ex.Message}");
                }
            }
            if(!dbContext.Platforms.Any())
            {
                Console.WriteLine("->> Seeding data");
                dbContext.Platforms.AddRange(
                    new Platform() { Name=". Net",Publisher="Microsoft",Cost="Free"},
                    new Platform() { Name = "SQL Server", Publisher = "Microsoft", Cost = "Free" },
                    new Platform() { Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free" }
                    );
                dbContext.SaveChanges();
            }
            else
            {
                Console.WriteLine("->> We already have data");
            }
        }
    }
}
