using Microsoft.AspNetCore.Http;
using OwlStock.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlStock.Services.DTOs.PhotoShoot
{
    public class AddFilesToPhotoShootDTO
    {
        public int PhotoShootId { get; set; }
        public string? PersonFullName { get; set; }
        public List<IFormFile>? FormFiles { get; set; }
    }
}
