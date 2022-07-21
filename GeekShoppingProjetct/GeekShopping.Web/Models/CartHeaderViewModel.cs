namespace GeekShopping.Web.Models
{
    public class CartHeaderViewModel
    {
        public long Id { get; set; }
        public string userId { get; set; }
        public string couponCode { get; set; }
        public decimal PurchaseAmount { get; set; }
    }
}
