using Microsoft.EntityFrameworkCore;
using PlatformWebService.Models;

namespace PlatformWebService.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt):base(opt)
        {

        }

        public DbSet<Platform> Platforms { get; set; }
    }
}
