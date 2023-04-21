using OwlStock.Domain.Entities;

namespace OwlStock.Services.Interfaces
{
    public interface IOrderService
    {
        Task<List<Order>> All();
        Task<List<Order>> All(string userId);
        Task<Order> GetById(int id);
        Task<int> CreateOrder(Order order);
    }
}
