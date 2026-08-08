using OwlStock.Domain.Entities;
using OwlStock.Services.DTOs.HomePage;

namespace OwlStock.Services.Interfaces
{
    public interface IHomeService
    {
        /// <summary>
        /// Gets list of the last four DynamicContents and Testimonies that are shown on the home page
        /// </summary>
        /// <param name="dynamicContents">List of last four DynamicContents</param>
        /// <param name="testimonies">List of last four Testimonies</param>
        /// <returns>HomePageDTO with the lists</returns>
        Task<HomePageDTO> GetHomeData(IEnumerable<Article> dynamicContents, IEnumerable<Testimony> testimonies);
    }
}
