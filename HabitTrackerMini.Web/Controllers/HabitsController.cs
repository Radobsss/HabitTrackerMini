
using HabitTrackerMini.Services.Contracts;
using HabitTrackerMini.Services.Models;
using HabitTrackerMini.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace HabitTrackerMini.Web.Controllers
{
    public class HabitsController : Controller
    {
        private readonly IHabitService habitService;

        public HabitsController(IHabitService habitService)
        {
            this.habitService = habitService;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var habits = await habitService.GetAllAsync(cancellationToken);
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

            await habitService.CreateAsync(input.Name, input.Category, input.WeeklyGoal,
                input.Description, cancellationToken);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
        {
            var details = await habitService.GetDetailsAsync(id, cancellationToken);
            if (details is null)
            {
                return NotFound();
            }

            return View(details);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Complete(int id, CancellationToken cancellationToken)
        {
            var result = await habitService.CompleteTodayAsync(id, cancellationToken);
            if (result == HabitCompletionResult.NotFound)
            {
                return NotFound();
            }

            TempData["CompletionMessage"] = result switch
            {
                HabitCompletionResult.Recorded => "Отбеляза навика като изпълнен за днес.",
                HabitCompletionResult.AlreadyRecorded => "Вече си отбелязал този навик за днес.",
                HabitCompletionResult.Inactive => "Можеш да отбелязваш само активни навици.",
                _ => throw new InvalidOperationException("Unexpected habit completion result.")
            };

            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
