namespace GeekShopping.Web.Models
{
    public class ProductModel
    {
        private string _name;

        public long Id { get; set; }

        public string Name { get => _name; set => _name = value; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public string CategoryName { get; set; }

        public string ImageUrl { get; set; }
    }
}
