using Microsoft.AspNetCore.Mvc;

namespace signalr.client.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
