using HabitTrackerMini.Data;
using HabitTrackerMini.Data.Entities;
using HabitTrackerMini.Services.Contracts;
using HabitTrackerMini.Services.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace HabitTrackerMini.Services;

public class HabitService : IHabitService
{
    private readonly HabitDbContext context;
    private readonly TimeProvider clock;

    public HabitService(HabitDbContext context, TimeProvider clock)
    {
        this.context = context;
        this.clock = clock;
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

    public async Task<HabitDetails?> GetDetailsAsync(int id, CancellationToken cancellationToken)
    {
        var habit = await context.Habits.AsNoTracking()
            .SingleOrDefaultAsync(item => item.Id == id, cancellationToken);
        if (habit is null)
        {
            return null;
        }

        var dates = await context.HabitEntries.AsNoTracking()
            .Where(entry => entry.HabitId == id)
            .OrderByDescending(entry => entry.Date)
            .Select(entry => entry.Date)
            .ToListAsync(cancellationToken);

        var today = DateOnly.FromDateTime(clock.GetLocalNow().DateTime);
        var daysSinceMonday = ((int)today.DayOfWeek + 6) % 7;
        var weekStart = today.AddDays(-daysSinceMonday);
        var weekEnd = weekStart.AddDays(6);
        var daysCompleted = dates.Count(date => date >= weekStart && date <= weekEnd);

        return new HabitDetails(habit, dates, weekStart, daysCompleted, dates.Contains(today));
    }

    public async Task<HabitCompletionResult> CompleteTodayAsync(int id, CancellationToken cancellationToken)
    {
        var habit = await context.Habits.AsNoTracking()
            .SingleOrDefaultAsync(item => item.Id == id, cancellationToken);
        if (habit is null)
        {
            return HabitCompletionResult.NotFound;
        }

        if (!habit.IsActive)
        {
            return HabitCompletionResult.Inactive;
        }

        var today = DateOnly.FromDateTime(clock.GetLocalNow().DateTime);
        if (await context.HabitEntries.AnyAsync(entry => entry.HabitId == id && entry.Date == today,
                cancellationToken))
        {
            return HabitCompletionResult.AlreadyRecorded;
        }

        var completion = new HabitEntry { HabitId = id, Date = today };
        context.HabitEntries.Add(completion);
        try
        {
            await context.SaveChangesAsync(cancellationToken);
            return HabitCompletionResult.Recorded;
        }
        catch (DbUpdateException exception) when (exception.InnerException is SqlException { Number: 2601 or 2627 })
        {
            // The unique index also protects requests that reach SaveChanges at the same time.
            context.Entry(completion).State = EntityState.Detached;
            return HabitCompletionResult.AlreadyRecorded;
        }
    }
}
