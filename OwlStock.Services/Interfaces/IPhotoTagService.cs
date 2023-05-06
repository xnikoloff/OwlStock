namespace OwlStock.Services.Interfaces
{
    public interface IPhotoTagService
    {
        Task<int> Add(string tags, int photoId);
    }
}
