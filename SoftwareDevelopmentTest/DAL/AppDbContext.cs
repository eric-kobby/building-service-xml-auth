using Microsoft.EntityFrameworkCore;
using SoftwareDevelopmentTest.DAL.Entities;

namespace SoftwareDevelopmentTest.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options){}

        public DbSet<Floor> Floors { get; set; }
        public DbSet<Fixture> Fixtures { get; set; }
    }
}

