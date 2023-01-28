using Microsoft.AspNetCore.Mvc;
using OwlStock.Domain;
using OwlStock.Services.Interfaces;
using System.Runtime.CompilerServices;

namespace OwlStock.Web.Controllers
{
    public class PhotoController : Controller
    {
        private readonly IPhotoService _service;

        public PhotoController(IPhotoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            return View(await _service.All());
        }

        [HttpGet]
        public async Task<IActionResult> PhotoById(int? id)
        {
            return View(await _service.GetById(id));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Photo? photo)
        {
            if (!ModelState.IsValid)
            {
                return View(photo);
            }

            await _service.Create(photo);

            return RedirectToAction(nameof(All));
        }
    }
}
