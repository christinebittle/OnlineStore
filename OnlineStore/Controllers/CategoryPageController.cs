using Microsoft.AspNetCore.Mvc;
using OnlineStore.Interfaces;
using OnlineStore.Models;
using OnlineStore.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace OnlineStore.Controllers
{

    public class CategoryPageController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;

        // dependency injection of service interface
        public CategoryPageController(ICategoryService CategoryService, IProductService ProductService)
        {
            _categoryService = CategoryService;
            _productService = ProductService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        // GET: CategoryPage/List
        [HttpGet]
        public async Task<IActionResult> List()
        {
            IEnumerable<CategoryDto?> CategoryDtos = await _categoryService.ListCategories();
            return View(CategoryDtos);
        }

        // GET: CategoryPage/Details/{id}
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            CategoryDto? CategoryDto = await _categoryService.FindCategory(id);
            IEnumerable<ProductDto> AssociatedProducts = await _productService.ListProductsForCategory(id);

            if (CategoryDto == null)
            {
                return View("Error", new ErrorViewModel() { Errors = ["Could not find category"] });
            }
            else
            {
                // information which drives a category page
                CategoryDetails CategoryInfo = new CategoryDetails()
                {
                    Category = CategoryDto,
                    CategoryProducts = AssociatedProducts
                };
                return View(CategoryInfo);
            }
        }

        // GET CategoryPage/New
        [Authorize(Roles = "admin")]
        public ActionResult New()
        {
            return View();
        }


        // POST CategoryPage/Add
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Add(CategoryDto CategoryDto)
        {
            ServiceResponse response = await _categoryService.AddCategory(CategoryDto);

            if (response.Status == ServiceResponse.ServiceStatus.Created)
            {
                return RedirectToAction("Details","CategoryPage", new { id = response.CreatedId });
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }

        //GET CategoryPage/Edit/{id}
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id)
        {
            CategoryDto? CategoryDto = await _categoryService.FindCategory(id);
            if (CategoryDto == null)
            {
                return View("Error");
            }
            else
            {
                return View(CategoryDto);
            }
        }

        //POST CategoryPage/Update/{id}
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Update(int id, CategoryDto CategoryDto)
        {
            ServiceResponse response = await _categoryService.UpdateCategory(CategoryDto);

            if (response.Status == ServiceResponse.ServiceStatus.Updated)
            {
                return RedirectToAction("Details", "CategoryPage", new { id = id });
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }

        //GET CategoryPage/ConfirmDelete/{id}
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            CategoryDto? CategoryDto = await _categoryService.FindCategory(id);
            if (CategoryDto == null)
            {
                return View("Error");
            }
            else
            {
                return View(CategoryDto);
            }
        }

        //POST CategoryPage/Delete/{id}
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            ServiceResponse response = await _categoryService.DeleteCategory(id);

            if (response.Status == ServiceResponse.ServiceStatus.Deleted)
            {
                return RedirectToAction("List", "CategoryPage");
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }
    }
}
