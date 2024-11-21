using Microsoft.AspNetCore.Mvc;
using OnlineStore.Interfaces;
using OnlineStore.Models.ViewModels;
using OnlineStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Ganss.Xss;


namespace OnlineStore.Controllers
{
    public class ProductPageController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IOrderItemService _orderItemService;
        

        //determine if user is admin
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        // dependency injection of service interface
        public ProductPageController(IProductService ProductService, ICategoryService CategoryService, IOrderItemService OrderItemService, UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
           
            _productService = ProductService;
            _categoryService = CategoryService;
            _orderItemService = OrderItemService;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            
        }

        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        // GET: ProductPage/List?PageNum={pagenum}
        // GET: ProductPage/List
        [HttpGet]
        public async Task<IActionResult> List(int PageNum=0)
        {
            // infer start and skip values from pagenum
            // assume 4 records per page in this example
            int PerPage = 4;
            // Determines the maximum number of pages (rounded up), assuming a page 0 start.
            int MaxPage = (int)Math.Ceiling((decimal) await _productService.CountProducts() / PerPage) - 1;

            // Lower boundary for Max Page
            if (MaxPage < 0) MaxPage = 0;
            // Lower boundary for Page Number
            if (PageNum < 0) PageNum = 0;
            // Upper Bound for Page Number
            if (PageNum > MaxPage) PageNum = MaxPage;

            // The Record Index of our Page Start
            int StartIndex = PerPage * PageNum;

            IEnumerable<ProductDto?> ProductDtos = await _productService.ListProducts(StartIndex,PerPage);

            ProductList ViewModel = new ProductList();
            ViewModel.Products = ProductDtos;
            ViewModel.MaxPage = MaxPage; 
            ViewModel.Page = PageNum;

            IdentityUser? User = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            if (User != null) ViewModel.isAdmin = await _userManager.IsInRoleAsync(User, "admin");
            else ViewModel.isAdmin = false;


            // Directs to /ProductPage/List.cshtml
            return View(ViewModel);
        }

        // GET: ProductPage/Details/{id}
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            ProductDto? ProductDto = await _productService.FindProduct(id);    


            IEnumerable<CategoryDto> AssociatedCategories = await _categoryService.ListCategoriesForProduct(id);
            IEnumerable<CategoryDto> Categories = await _categoryService.ListCategories();

            //need the ordered items for this product
            IEnumerable<OrderItemDto?> OrderItems = await _orderItemService.ListOrderItemsForProduct(id);

            if (ProductDto == null)
            {
                return View("Error", new ErrorViewModel() { Errors = ["Could not find Product"] });
            }
            else
            {
                // information which drives a Product page
                ProductDetails ProductInfo = new ProductDetails()
                {
                    Product = ProductDto,
                    ProductCategories = AssociatedCategories,
                    AllCategories = Categories,
                    ProductOrderedItems = OrderItems
                };
                return View(ProductInfo);
            }
        }

        // GET ProductPage/New
        [Authorize(Roles = "admin")]
        public ActionResult New()
        {
            return View();
        }


        // POST ProductPage/Add
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Add(ProductDto ProductDto)
        {
            ServiceResponse response = await _productService.AddProduct(ProductDto);

            if (response.Status == ServiceResponse.ServiceStatus.Created)
            {
                return RedirectToAction("Details", "ProductPage", new { id = response.CreatedId });
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }

        //GET ProductPage/Edit/{id}
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id)
        {
            ProductDto? ProductDto = await _productService.FindProduct(id);
            if (ProductDto == null)
            {
                return View("Error");
            }
            else
            {
                return View(ProductDto);
            }
        }

        //POST ProductPage/Update/{id}
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Update(int id, ProductDto ProductDto, IFormFile ProductPic)
        {
            ServiceResponse imageresponse = await _productService.UpdateProductImage(id, ProductPic);

            ServiceResponse response = await _productService.UpdateProduct(ProductDto);

            if (response.Status == ServiceResponse.ServiceStatus.Updated)
            {
                return RedirectToAction("Details", "ProductPage", new { id = id });
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }

        //GET ProductPage/ConfirmDelete/{id}
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            ProductDto? ProductDto = await _productService.FindProduct(id);
            if (ProductDto == null)
            {
                return View("Error");
            }
            else
            {
                return View(ProductDto);
            }
        }

        //POST ProductPage/Delete/{id}
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            ServiceResponse response = await _productService.DeleteProduct(id);

            if (response.Status == ServiceResponse.ServiceStatus.Deleted)
            {
                return RedirectToAction("List", "ProductPage");
            }
            else
            {
                return RedirectToAction("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }

        //POST ProductPage/LinkToCategory
        //DATA: categoryId={categoryId}&productId={productId}
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> LinkToCategory([FromForm]int productId, [FromForm]int categoryId)
        {
            await _categoryService.LinkCategoryToProduct(categoryId, productId);

            return RedirectToAction("Details", new { id = productId });
        }

        //POST ProductPage/UnlinkFromCategory
        //DATA: categoryId={categoryId}&productId={productId}
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UnlinkFromCategory([FromForm] int productId, [FromForm] int categoryId)
        {
            await _categoryService.UnlinkCategoryFromProduct(categoryId, productId);

            return RedirectToAction("Details", new { id = productId });
        }
    }
}
