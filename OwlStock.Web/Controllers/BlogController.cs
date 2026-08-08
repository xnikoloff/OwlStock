using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OwlStock.Domain.Entities;
using OwlStock.Services.DTOs.Articles;
using OwlStock.Services.Facades.Interfaces;
using OwlStock.Services.Interfaces;
using System.Security.Claims;

namespace OwlStock.Web.Controllers
{
    [Route("blog")]
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly IBlogServiceFacade _blogServiceFacade;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BlogController(IBlogService blogService, IBlogServiceFacade blogServiceFacade, IWebHostEnvironment webHostEnvironment)
        {
            _blogService = blogService;
            _blogServiceFacade = blogServiceFacade;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("statiya")]
        public async Task<IActionResult> Content(Guid id)
        {
            Article content = await _blogService.GetById(id);

            if (content.Id == Guid.Empty)
            {
                return View("Error", "Не успяхме да намерим съдържанието, която търсите");
            }

            return View(content);
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            return View(await _blogService.GetAll());
        }

        [HttpGet("allByPage")]
        public async Task<IActionResult> AllByPage(int pageNumber = 1)
        {
            ViewData["PageNumber"] = pageNumber;
            var all = await _blogService.GetAllByPage(pageNumber);

            return View(nameof(Index), all);
        }

        [HttpGet("allByCategory")]
        public async Task<IActionResult> AllByCategory(Guid id)
        {
            AllArticlesDTO all = await _blogService.GetAllByCategory(id);

            if (all.Articles == null || all.ArticleCategories == null)
            {
                return View("Error", "Опитайте пак по-късно");
            }

            return View(nameof(Index), all);
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet("allDeleted")]
        public async Task<IActionResult> AllDeleted(Guid id)
        {
            AllArticlesDTO all = await _blogService.GetAllDeleted();

            if (all.Articles == null || all.ArticleCategories == null)
            {
                return View("Error", "Опитайте пак по-късно");
            }

            return View(nameof(Index), all);
        }

        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            List<ArticleCategory> categories =
                (await _blogService.GetAllArticleCategories()).ToList();

            return View(new CreateArticleDTO()
            {
                ArticleCategories = categories
            });
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateArticleDTO dto)
        {
            dto.WebRootPath = _webHostEnvironment.WebRootPath;
            dto.Article.CreatedById = User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                throw new NullReferenceException("User not logged in");

            bool isSuccessful = await _blogServiceFacade.Create(dto);

            if (!isSuccessful)
            {
                return View("Error", "An error occured while creating the dynamic content. See the log for details");
            }
            return RedirectToAction(nameof(AllByPage));
        }

        [HttpGet("delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            bool isSuccessful = await _blogService.Delete(id);    

            if (!isSuccessful)
            {
                return View("Error", "An error occured while deleting the dynamic content. See the log for details");
            }

            return RedirectToAction(nameof(AllByPage));
        }

        [HttpGet("recover")]
        public async Task<IActionResult> Recover(Guid id)
        {
            bool isSuccessful = await _blogService.Recover(id);

            if (!isSuccessful)
            {
                return View("Error", "An error occured while recovering the dynamic content. See the log for details");
            }

            return RedirectToAction(nameof(AllByPage));
        }

        [HttpGet("allCategories")]
        public async Task<IActionResult> AllCategories()
        {
            return View(await _blogService.GetAllArticleCategories());
        }

        [HttpGet("createCategory")]
        public async Task<IActionResult> CreateCategory()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost("createCategory")]
        public async Task<IActionResult> CreateCategory(ArticleCategory category)
        {
            bool result = await _blogService.CreateCategory(category);

            if (result)
            {
                return RedirectToAction(nameof(AllCategories));
            }

            else
            {
                return View("Error", "Cannot create category");
            }
        }
    }
}
