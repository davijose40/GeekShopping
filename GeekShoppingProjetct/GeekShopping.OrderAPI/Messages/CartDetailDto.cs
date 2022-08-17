namespace GeekShopping.OrderAPI.Messages
{
    public class CartDetailDto
    {
        public long Id { get; set; }

        public long CartHeaderId { get; set; }

        public long ProductId { get; set; }

        public virtual ProductDto Product { get; set; }

        public int Count { get; set; }
    }
}
