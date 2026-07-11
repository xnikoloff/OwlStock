using OwlStock.Domain.Entities;

namespace OwlStock.Services.Interfaces
{
    public interface IGiftCardService
    {
        Task<IEnumerable<GiftCard>> GetAll();
        Task<GiftCard> GetById(Guid id);
        Task<bool> Create(GiftCard giftCard);
    }
}
