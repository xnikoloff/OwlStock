using Microsoft.AspNetCore.Http;
using OwlStock.Domain.Entities;
using OwlStock.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlStock.Services
{
    public interface IFileService
    {
        Task<int> Create(IFormFile? file, Photo photo, string? webRootPath, PhotoSize size, string fileName);
        void Create(List<IFormFile> file, string? webRootPath, PhotoSize size);
    }
}
