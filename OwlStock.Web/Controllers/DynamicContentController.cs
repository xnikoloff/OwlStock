using Microsoft.AspNetCore.Mvc;
using OwlStock.Domain.Entities;
using OwlStock.Services.DTOs;
using OwlStock.Services.Interfaces;

namespace OwlStock.Web.Controllers
{
    public class DynamicContentController : Controller
    {
        private readonly IDynamicContentService _dynamicContentService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DynamicContentController(IDynamicContentService dynamicContentService, IWebHostEnvironment webHostEnvironment)
        {
            _dynamicContentService = dynamicContentService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Content(Guid id)
        {
            return View(await _dynamicContentService.GetById(id));
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            return View(await _dynamicContentService.GetAll());
        }

        [HttpGet]
        public  IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateDynamicContentDTO dto)
        {
            dto.WebRootPath = _webHostEnvironment.WebRootPath;
            DynamicContent dynamicContent = await _dynamicContentService.Create(dto);
            return RedirectToAction(nameof(Content), new { id = dynamicContent?.Id });
        }
    }
}
