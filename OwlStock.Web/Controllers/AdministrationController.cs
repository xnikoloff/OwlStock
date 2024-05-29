using Microsoft.AspNetCore.Mvc;

namespace OwlStock.Web.Controllers
{
    public class AdministrationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
