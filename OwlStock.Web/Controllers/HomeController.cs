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
            ViewBag.HomePhoto = await _homeService.ChooseHomePagePhoto();
            IEnumerable<DynamicContent> dynamicContents = await _dynamicContentService.GetAll();
            
            return View(dynamicContents);
        }
    }
}
