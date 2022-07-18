using System.ComponentModel.DataAnnotations;

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

        [Range(1, 100)]
        public int Count { get; set; } = 1;

        public string SubstringName()
        {
            if(Name.Length < 24) return Name;

            return $"{Name[..21]} ...";
        }

        public string SubstringDescription()
        {
            if (Description.Length < 355) return Description;

            return $"{Description[..352]} ...";
        }
    }
}
