using OwlStock.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlStock.Services.Interfaces
{
    public interface ICategoryService
    {
        string GetCategoryDescription(Category category);
    }
}
