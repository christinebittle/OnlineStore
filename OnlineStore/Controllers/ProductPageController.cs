using Microsoft.AspNetCore.Mvc;
using OnlineStore.Interfaces;
using OnlineStore.Models.ViewModels;
using OnlineStore.Models;

namespace OnlineStore.Controllers
{
    public class ProductPageController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IOrderItemService _orderItemService;

        // dependency injection of service interface
        public ProductPageController(IProductService ProductService, ICategoryService CategoryService, IOrderItemService OrderItemService)
        {
           
            _productService = ProductService;
            _categoryService = CategoryService;
            _orderItemService = OrderItemService;
        }

        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        // GET: ProductPage/List
        public async Task<IActionResult> List()
        {
            IEnumerable<ProductDto?> ProductDtos = await _productService.ListProducts();
            return View(ProductDtos);
        }

        // GET: ProductPage/Details/{id}
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            ProductDto? ProductDto = await _productService.FindProduct(id);
            IEnumerable<CategoryDto> AssociatedCategories = await _categoryService.ListCategoriesForProduct(id);
            IEnumerable<CategoryDto> Categories = await _categoryService.ListCategories();

            //need the ordered items for this product
            IEnumerable<OrderItemDto> OrderItems = await _orderItemService.ListOrderItemsForProduct(id);

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
        public ActionResult New()
        {
            return View();
        }


        // POST ProductPage/Add
        [HttpPost]
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
        public async Task<IActionResult> Update(int id, ProductDto ProductDto)
        {
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
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            // Views/ProductPage/ConfirmDelete.cshtml
            // find information about this product
            ProductDto? ProductDto = await _productService.FindProduct(id);

            return View(ProductDto);
        }

        //POST ProductPage/Delete/{id}
        [HttpPost]
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
        public async Task<IActionResult> LinkToCategory([FromForm]int productId, [FromForm]int categoryId)
        {
            await _categoryService.LinkCategoryToProduct(categoryId, productId);

            return RedirectToAction("Details", new { id = productId });
        }

        //POST ProductPage/UnlinkFromCategory
        //DATA: categoryId={categoryId}&productId={productId}
        [HttpPost]
        public async Task<IActionResult> UnlinkFromCategory([FromForm] int productId, [FromForm] int categoryId)
        {
            await _categoryService.UnlinkCategoryFromProduct(categoryId, productId);

            return RedirectToAction("Details", new { id = productId });
        }
    }
}
