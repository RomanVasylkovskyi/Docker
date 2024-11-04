namespace WebHalk.Models.Products
{
    public class ProductHomeViewModel
    {
        //Знайдені продукти, для поточної сторінки
        public List<ProductItemViewModel>? Products { get; set; }
        //Параметри для пошуку, які було використано
        public ProductSearchViewModel? Search { get; set; }
        //Інформація про пагінацію 1 2 3 4 ...
        public PaginationViewModel? Pagination { get; set; }
        //Загальна кількість знайдених елементів усіх
        public int Count { get; set; }
    }
}
