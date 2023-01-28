using Microsoft.AspNetCore.Mvc;

namespace OwlStock.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
