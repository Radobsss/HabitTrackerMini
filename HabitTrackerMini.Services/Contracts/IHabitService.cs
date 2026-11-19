using HabitTrackerMini.Data.Entities;
using HabitTrackerMini.Services.Models;

namespace HabitTrackerMini.Services.Contracts;

public interface IHabitService
{
    Task<IReadOnlyList<Habit>> GetAllAsync(CancellationToken cancellationToken);

    Task CreateAsync(string name, string category, int weeklyGoal, string? description,
        CancellationToken cancellationToken);

    Task<HabitDetails?> GetDetailsAsync(int id, CancellationToken cancellationToken);

    Task<HabitCompletionResult> CompleteTodayAsync(int id, CancellationToken cancellationToken);
}
