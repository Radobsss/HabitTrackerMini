using HabitTrackerMini.Data;
using HabitTrackerMini.Data.Entities;
using HabitTrackerMini.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace HabitTrackerMini.Services;

public class HabitService : IHabitService
{
    private readonly HabitDbContext context;

    public HabitService(HabitDbContext context)
    {
        this.context = context;
    }

    public async Task<IReadOnlyList<Habit>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Habits.AsNoTracking()
            .OrderByDescending(habit => habit.IsActive)
            .ThenBy(habit => habit.Name)
            .ThenBy(habit => habit.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task CreateAsync(string name, string category, int weeklyGoal, string? description,
        CancellationToken cancellationToken)
    {
        context.Habits.Add(new Habit
        {
            Name = name.Trim(),
            Category = category.Trim(),
            WeeklyGoal = weeklyGoal,
            Description = description?.Trim() ?? string.Empty
        });
        await context.SaveChangesAsync(cancellationToken);
    }
}
