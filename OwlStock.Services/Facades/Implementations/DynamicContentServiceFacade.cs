using OwlStock.Services.DTOs.Articles;
using OwlStock.Services.Facades.Interfaces;
using OwlStock.Services.Interfaces;

namespace OwlStock.Services.Facades.Implementations
{
    public class BlogServiceFacade : IBlogServiceFacade
    {
        private readonly IBlogService _blogService;
        private readonly ICalculationsService _calculationsService;
        private readonly IFileService _fileService;

        public BlogServiceFacade(IBlogService blogService, ICalculationsService calculationsService, IFileService fileService)
        {
            _blogService = blogService;
            _calculationsService = calculationsService;
            _fileService = fileService;
        }

        public async Task<bool> Create(CreateArticleDTO dto)
        {
            dto!.Article!.ReadingTime = _calculationsService.CalculateReadingTime(dto.Article.Content);

            bool resultIFormFile = await _fileService.CreateIFormFile(dto!.Image, dto!.WebRootPath);

            if (!resultIFormFile)
            {
                return false;
            }

            bool resultContent = await _blogService.Create(dto);

            if (!resultContent)
            {
                return false;
            }

            return true;
        }
    }
}
