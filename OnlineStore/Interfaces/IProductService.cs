using OnlineStore.Models;

namespace OnlineStore.Interfaces
{
    public interface IProductService
    {
        // base CRUD
        Task<IEnumerable<ProductDto>> ListProducts();


        Task<ProductDto?> FindProduct(int id);


        Task<ServiceResponse> UpdateProduct(ProductDto productDto);

        Task<ServiceResponse> AddProduct(ProductDto productDto);

        Task<ServiceResponse> DeleteProduct(int id);

        // related methods

        Task<IEnumerable<ProductDto>> ListProductsForCategory(int id);
    }
}
