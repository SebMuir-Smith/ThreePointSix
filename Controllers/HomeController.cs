using Microsoft.AspNetCore.Mvc;

namespace ThreePointSix.Controllers {
    public class HomeController : Controller {
        public IActionResult Index() {
            return View();
        }
    }
}