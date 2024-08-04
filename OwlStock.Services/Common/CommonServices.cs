
using OwlStock.Infrastructure.Common;

namespace OwlStock.Services.Common
{
    public class CommonServices : ICommonServices
    {
        public string GetEnumDescription(Enum enumeration)
        {
            if (string.IsNullOrEmpty(enumeration.ToString()))
            {
                throw new ArgumentNullException(nameof(enumeration));
            }

            CategoryDescriptions enumerationDescriptions = new();

            object field = enumerationDescriptions
                .GetType()
                .GetFields()
                .Where(f => f.Name.Contains(enumeration.ToString()))
                .Select(f => f.GetValue(enumerationDescriptions))
                .FirstOrDefault() ?? throw new NullReferenceException($"Member that contains name {enumeration} does not exists");

            return field.ToString() ?? "N/A";
        }
    }
}
