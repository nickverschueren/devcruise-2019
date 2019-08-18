using Microsoft.EntityFrameworkCore;

namespace Euricom.DevCruise.Model
{
    public partial class DevCruiseDbContext : DbContext
    {
        public DevCruiseDbContext(DbContextOptions<DevCruiseDbContext> options) : base(options)
        {
            if (Database.EnsureCreated()) Initialize();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var session = modelBuilder.Entity<Session>();
            session.HasKey(s => new { s.Id });
            session.HasIndex(s => s.Code).IsUnique();

            var slot = modelBuilder.Entity<Slot>();
            slot.HasKey(s => s.Id);
            slot.HasMany(s => s.SlotSpeakers)
                .WithOne(s => s.Slot)
                .HasForeignKey(s => s.SlotId)
                .OnDelete(DeleteBehavior.Cascade);
            slot.HasOne(s => s.Session)
                .WithMany();
            slot.HasIndex(s => new { s.Room, s.StartTime }).IsUnique();

            var speaker = modelBuilder.Entity<Speaker>();
            speaker.HasKey(s => s.Id);
            speaker.HasMany<SlotSpeaker>()
                .WithOne(s => s.Speaker)
                .HasForeignKey(s => s.SpeakerId)
                .OnDelete(DeleteBehavior.Cascade);
            speaker.HasIndex(s => s.Email).IsUnique();

            var slotSpeaker = modelBuilder.Entity<SlotSpeaker>();
            slotSpeaker.HasKey(ss => new { ss.SlotId, ss.SpeakerId });
            slotSpeaker.HasIndex(ss => ss.SlotId);
            slotSpeaker.HasIndex(ss => ss.SpeakerId);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Session> Sessions { get; set; }
        public DbSet<Slot> Slots { get; set; }
        public DbSet<Speaker> Speakers { get; set; }
    }
}
