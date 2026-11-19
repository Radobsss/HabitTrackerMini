using HabitTrackerMini.Data.Entities;

namespace HabitTrackerMini.Services.Models;

public record HabitDetails(Habit Habit, IReadOnlyList<DateOnly> CompletionDates, DateOnly WeekStart,
    int DaysCompleted, bool CompletedToday);
