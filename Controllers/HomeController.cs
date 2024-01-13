using Microsoft.AspNetCore.Mvc;

namespace TaskHub.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
