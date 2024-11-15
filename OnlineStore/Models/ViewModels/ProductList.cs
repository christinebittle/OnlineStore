namespace OnlineStore.Models.ViewModels
{
    public class ProductList
    {

        public IEnumerable<ProductDto> Products { get; set; }

        public bool isAdmin { get; set; }

        public int Page { get; set; }

        public int MaxPage { get; set; }
    }
}
