using Microsoft.EntityFrameworkCore;
using OwlStock.Domain.Entities;
using OwlStock.Infrastructure;
using OwlStock.Services.Interfaces;

namespace OwlStock.Services
{
    internal class OrderService : IOrderService
    {
        private readonly OwlStockDbContext _context;

        public OrderService(OwlStockDbContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> All()
        {
            if(_context.Orders is null)
            {
                throw new NullReferenceException($"{nameof(_context.Orders)} is null");
            }
                
            return await _context.Orders.ToListAsync();
        }

        public async Task<List<Order>> All(string userId)
        {
            if (_context.Orders is null)
            {
                throw new NullReferenceException($"{nameof(_context.Orders)} is null");
            }

            return await _context.Orders
                .Where(o => o.IdentityUserId == userId)
                .ToListAsync();
        }

        public async Task<bool> CreateOrder(Order order)
        {
            await _context.AddAsync(order);

            int result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}
