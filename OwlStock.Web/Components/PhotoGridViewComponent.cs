using Microsoft.AspNetCore.Mvc;
using OwlStock.Domain.Entities;
using OwlStock.Services.Interfaces;

namespace OwlStock.Web.Components
{
    public class PhotoGridViewComponent : ViewComponent
    {
        private readonly IPhotoShootService _photoShootService;

        public PhotoGridViewComponent(IPhotoShootService photoShootService)
        {
            _photoShootService= photoShootService;
        }

        public async Task<IActionResult> InvokeAsync(Guid id)
        {
            throw new NotImplementedException();
            //IEnumerable<Photo> photos = await _photoShootService.PhotoShootById(id);
        }
    }
}
