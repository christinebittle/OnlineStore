namespace OnlineStore.Models.ViewModels
{
    public class CategoryViewModel
    {
        //A category page must have a category
        public required CategoryDto Category {get;set;}

        //A category page can have many products
        public IEnumerable<ProductDto>? CategoryProducts { get; set; }
    }
}
