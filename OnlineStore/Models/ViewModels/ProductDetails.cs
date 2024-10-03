namespace OnlineStore.Models.ViewModels
{
    public class ProductDetails
    {
        // A product page must have a product
        // FindProduct(productid)
        public required ProductDto Product { get; set; }

        // A product may have categories associated to it
        // ListCategoriesForProduct(productid)
        public IEnumerable<CategoryDto>? ProductCategories { get; set; }

        // All categories
        // ListCategories()
        public IEnumerable<CategoryDto>? AllCategories { get; set; }

        // All order items for this product
        public IEnumerable<OrderItemDto>? ProductOrderedItems { get; set; }

    }
}
