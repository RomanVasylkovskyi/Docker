namespace WebHalk.Models.Products
{
    public class ProductSearchViewModel
    {
        public string? Name { get; set; }
        public int CategoryId { get; set; }
        public int? Page { get; set; }
        public int PageSize { get; set; } = 10;
    }
}
