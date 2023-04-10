using Microsoft.EntityFrameworkCore;
using SimpleLMS.API.Models;

namespace SimpleLMS.API.Data
{
    public class SimpleLMSContext : DbContext
    {
        public SimpleLMSContext(DbContextOptions<SimpleLMSContext> options) : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; } = null!;
        public DbSet<Module> Modules { get; set; } = null!;
        public DbSet<Assignment> Assignments { get; set; } = null!;
    }
}
