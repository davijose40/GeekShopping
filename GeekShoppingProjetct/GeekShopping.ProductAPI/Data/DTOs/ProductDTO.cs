namespace GeekShopping.ProductAPI.Data.DTOs
{
    public class ProductDTO
    {
        private string name;

        public long Id { get; set; }

        public string Name { get => name; set => name = value; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public string CategoryName { get; set; }

        public string ImageUrl { get; set; }
    }
}
