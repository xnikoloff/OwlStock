using OwlStock.Domain.Entities;
using OwlStock.Services.DTOs.HomePage;
using OwlStock.Services.Facades.Interfaces;
using OwlStock.Services.Interfaces;

namespace OwlStock.Services.Facades.Implementations
{
    public class HomeFacade : IHomeFacade
    {
        private readonly IHomeService _homeService;
        private readonly IBlogService _dynamicContentService;
        private readonly ITestimonyService _testimonyService;

        public HomeFacade(IHomeService homeService, IBlogService dynamicContentService, ITestimonyService testimonyService)
        {
            _homeService = homeService;
            _dynamicContentService = dynamicContentService;
            _testimonyService = testimonyService;
        }

        public async Task<HomePageDTO> GetHomeData()
        {
            return await _homeService.GetHomeData(await GetDynamicContents(), await GetTestimonies());
        }

        private async Task<IEnumerable<Article>> GetDynamicContents()
        {
            return await _dynamicContentService.GetTopContent();
        }

        private async Task<IEnumerable<Testimony>> GetTestimonies()
        {
            return await _testimonyService.GetLastFour();
        }
    }
}
