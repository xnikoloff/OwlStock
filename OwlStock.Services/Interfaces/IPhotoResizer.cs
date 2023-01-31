using OwlStock.Domain;
using OwlStock.Domain.Enumerations;

namespace OwlStock.Services.Interfaces
{
    public interface IPhotoResizer
    {
        Photo Resize(Photo photo, PhotoSize photoSize);   
    } 
}
