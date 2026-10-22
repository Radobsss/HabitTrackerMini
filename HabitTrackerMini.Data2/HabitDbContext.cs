using HabitTrackerMini.Data2.Entities;
using Microsoft.EntityFrameworkCore;

namespace HabitTrackerMini.Data
{
    public class HabitDbContext : DbContext
    {
        public HabitDbContext(DbContextOptions<HabitDbContext> options)
            : base(options)
        {
        }

        public DbSet<Habit> Habits { get; set; }
    }
}