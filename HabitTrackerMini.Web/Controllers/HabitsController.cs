
using HabitTrackerMini.Data;
using HabitTrackerMini.Data.Entities;
using HabitTrackerMini.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HabitTrackerMini.Web.Controllers
{
    public class HabitsController : Controller
    {
        private readonly HabitDbContext context;

        public HabitsController(HabitDbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var habits = await context.Habits.AsNoTracking()
                .OrderByDescending(habit => habit.IsActive)
                .ThenBy(habit => habit.Name)
                .ThenBy(habit => habit.Id)
                .ToListAsync(cancellationToken);

            return View(habits);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreateHabitInput());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateHabitInput input, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            context.Habits.Add(new Habit
            {
                Name = input.Name.Trim(),
                Category = input.Category.Trim(),
                WeeklyGoal = input.WeeklyGoal,
                Description = input.Description?.Trim() ?? string.Empty
            });
            await context.SaveChangesAsync(cancellationToken);

            return RedirectToAction(nameof(Index));
        }
    }
}
