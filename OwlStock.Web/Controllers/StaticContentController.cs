using Microsoft.AspNetCore.Mvc;

namespace OwlStock.Web.Controllers
{
    public class StaticContentController : Controller
    {
        public IActionResult Services()
        {
            return View();
        }

        public IActionResult Pricing()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
    }
}
