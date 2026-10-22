using HabitTrackerMini.Data.Entities;
using HabitTrackerMini.Data2.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

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