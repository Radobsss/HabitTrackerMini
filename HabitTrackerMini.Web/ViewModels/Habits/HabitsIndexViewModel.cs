using HabitTrackerMini.Services.DTOs.Habits;

namespace HabitTrackerMini.Web.ViewModels.Habits;

public record HabitsIndexViewModel(IReadOnlyList<HabitListDto> Habits);
