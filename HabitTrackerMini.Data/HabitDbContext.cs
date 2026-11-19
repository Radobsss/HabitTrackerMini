using HabitTrackerMini.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace HabitTrackerMini.Data
{
    public class HabitDbContext : DbContext
    {
        public HabitDbContext(DbContextOptions<HabitDbContext> options)
            : base(options)
        {
        }

        public DbSet<Habit> Habits => Set<Habit>();

        public DbSet<HabitEntry> HabitEntries => Set<HabitEntry>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var habit = modelBuilder.Entity<Habit>();
            habit.HasKey(item => item.Id);
            habit.Property(item => item.Name).IsRequired();
            habit.Property(item => item.Category).IsRequired();
            habit.Property(item => item.Description).IsRequired();

            var entry = modelBuilder.Entity<HabitEntry>();
            entry.HasKey(item => item.Id);
            entry.Property(item => item.Date).HasColumnType("date");
            entry.HasIndex(item => new { item.HabitId, item.Date }).IsUnique();
            entry.HasOne(item => item.Habit)
                .WithMany(item => item.Entries)
                .HasForeignKey(item => item.HabitId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
