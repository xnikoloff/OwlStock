﻿using Microsoft.EntityFrameworkCore;
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
                .Include(o => o.Photo)
                .Where(o => o.IdentityUserId == userId)
                .ToListAsync();
        }

        public async Task<Order> GetById(int id)
        {
            if(_context.Orders is null)
            {
                throw new NullReferenceException($"{nameof(_context.Orders)} is null");
            }

            return await _context.Orders
                .Include(o => o.Photo)
                .Where(o => o.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Order> CreateOrder(Order order)
        {
            if(order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }

            order.PhotoId = order?.Photo?.Id;
            order!.Photo = null;

            if(_context.Orders is not null)
            {
                await _context.Orders.AddAsync(order);
            }
            
            await _context.SaveChangesAsync();

            return await _context.Orders
                .Include(o => o.Photo)
                    .ThenInclude(p => p.PhotoCategories)
                .OrderByDescending(o => o.Id)
                .FirstOrDefaultAsync();
        }
    }
}
