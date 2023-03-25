using OwlStock.Domain.Enumerations;
using OwlStock.Infrastructure.Common;
using OwlStock.Services.Interfaces;

namespace OwlStock.Services
{
    internal class CategoryService : ICategoryService
    {
        public string GetCategoryDescription(Category category)
        {
            if (string.IsNullOrEmpty(category.ToString()))
            {
                throw new ArgumentNullException(nameof(category));
            }

            CategoryDescriptions categoryDescriptions = new();

            object field = categoryDescriptions
                .GetType()
                .GetFields()
                .Where(f => f.Name.Contains(category.ToString()))
                .Select(f => f.GetValue(categoryDescriptions))
                .FirstOrDefault();

            if(field != null)
            {
                return field.ToString();
            }

            throw new NullReferenceException($"Member that contains name {category.ToString()} does not exists");
        }
    }
}
