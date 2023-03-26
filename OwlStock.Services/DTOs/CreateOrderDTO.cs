using OwlStock.Domain.Entities;
using OwlStock.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlStock.Services.DTOs
{
    public class CreateOrderDTO
    {
        public Order? Order { get; set; }
        public PhotoSize PhotoSize { get; set; }
    }
}
