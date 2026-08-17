using OwlStock.Domain.Entities;
using OwlStock.Domain.Enumerations;

namespace OwlStock.Services.Interfaces
{
    public interface IGiftCardService
    {
        Task<IEnumerable<GiftCard>> GetAll();
        Task<GiftCard> GetById(Guid id);
        Task<GiftCard> Create(GiftCard giftCard);
        Task<bool> ChangeStatus(Guid id, GiftCardStatus status);
        string GetHtmlTemplate(GiftCard giftCard);
    }
}
