using Microsoft.AspNetCore.Mvc;
using OwlStock.Domain.Entities;
using OwlStock.Services.Interfaces;

namespace OwlStock.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDynamicContentService _dynamicContentService;
        private readonly IHomeService _homeService;
        
        public HomeController(IDynamicContentService dynamicContentServic, IHomeService homeService)
        {
            _dynamicContentService = dynamicContentServic;
            _homeService = homeService;
        }

        public async Task<IActionResult> Index()
        {
            string photo = await _homeService.ChooseHomePagePhoto();
            if(System.IO.File.Exists(photo))
            {
                ViewBag.HomePhoto = photo;
            }
            
            ViewBag.HomePhoto = @Url.Content("~/resources/images/background.jpg");
            IEnumerable<DynamicContent> dynamicContents = await _dynamicContentService.GetLastFour();
            
            return View(dynamicContents);
        }
    }
}
