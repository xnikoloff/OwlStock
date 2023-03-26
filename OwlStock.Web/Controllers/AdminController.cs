using Microsoft.AspNetCore.Mvc;

namespace OwlStock.Web.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreatePhoto()
        {
            return View();
        }
    }
}
