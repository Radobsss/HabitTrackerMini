using HabitTrackerMini.Data.Entities;

namespace HabitTrackerMini.Services.Contracts;

public interface IHabitService
{
    Task<IReadOnlyList<Habit>> GetAllAsync(CancellationToken cancellationToken);

    Task CreateAsync(string name, string category, int weeklyGoal, string? description,
        CancellationToken cancellationToken);
}
