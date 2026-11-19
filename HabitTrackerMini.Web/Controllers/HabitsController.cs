
using HabitTrackerMini.Services.Contracts;
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
    }
}
