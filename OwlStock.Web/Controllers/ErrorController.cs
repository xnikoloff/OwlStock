using Microsoft.AspNetCore.Mvc;

namespace OwlStock.Web.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Error(string message)
        {
            return View(message);
        }
    }
}
