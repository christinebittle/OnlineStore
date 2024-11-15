using OnlineStore.Models;

namespace OnlineStore.Interfaces
{
    public interface IProductService
    {
        // base CRUD
        Task<IEnumerable<ProductDto>> ListProducts(int skip, int page);

        Task<int> CountProducts();

        Task<ProductDto?> FindProduct(int id);


        Task<ServiceResponse> UpdateProduct(ProductDto productDto);

        Task<ServiceResponse> AddProduct(ProductDto productDto);

        Task<ServiceResponse> DeleteProduct(int id);

        Task<ServiceResponse> UpdateProductImage(int id, IFormFile ProductPic);

        // related methods

        Task<IEnumerable<ProductDto>> ListProductsForCategory(int id);
    }
}
