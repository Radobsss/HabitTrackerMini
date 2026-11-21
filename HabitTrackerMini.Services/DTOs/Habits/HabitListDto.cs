namespace HabitTrackerMini.Services.DTOs.Habits;

public record HabitListDto(int Id, string Name, string Category, int WeeklyGoal, bool IsActive);
