namespace OnlineStore.Models.ViewModels
{
    public class ProductViewModel
    {
        //A product page must have a product
        public required ProductDto Product { get; set; }

        //A product may have categories associated to it
        public IEnumerable<CategoryDto>? ProductCategories { get; set; }

        //All categories
        public IEnumerable<CategoryDto>? AllCategories { get; set; }

    }
}
