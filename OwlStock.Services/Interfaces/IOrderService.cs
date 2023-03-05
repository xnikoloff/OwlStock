﻿using OwlStock.Domain.Entities;

namespace OwlStock.Services.Interfaces
{
    public interface IOrderService
    {
        Task<List<Order>> All();
        Task<List<Order>> All(string userId);
    }
}