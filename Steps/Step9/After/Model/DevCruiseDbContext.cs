using Microsoft.EntityFrameworkCore;

namespace DevCruise.Model
{
    public partial class DevCruiseDbContext : DbContext
    {
        public DevCruiseDbContext(DbContextOptions<DevCruiseDbContext> options) : base(options)
        {
            EnsureAppDataDirectory();
            if (Database.EnsureCreated()) Initialize();
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var session = modelBuilder.Entity<Session>();
            session.HasIndex(s => s.Code).IsUnique();

            var speaker = modelBuilder.Entity<Speaker>();
            speaker.HasIndex(s => s.Email).IsUnique();
        }

        public DbSet<Session> Sessions { get; set; } 
        public DbSet<Speaker> Speakers { get; set; } 
    }
}