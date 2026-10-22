
using Microsoft.AspNetCore.Mvc;

namespace HabitTrackerMini.Web.Controllers
{
    public class HabitsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }
    }
}
