namespace OnlineStore.Models.ViewModels
{
    public class ProductList
    {
        // This ViewModel is the structure needed for us to render
        // ProductPage/List.cshtml

        public IEnumerable<ProductDto> Products { get; set; }

        public bool isAdmin { get; set; }

        public int Page { get; set; }

        public int MaxPage { get; set; }
    }
}
