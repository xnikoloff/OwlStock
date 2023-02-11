using OwlStock.Domain;
using OwlStock.Domain.Enumerations;

namespace OwlStock.Services.Interfaces
{
    public interface IPhotoResizer
    {
        byte[] Resize(byte[] fileData, PhotoSize photoSize);   
    } 
}
