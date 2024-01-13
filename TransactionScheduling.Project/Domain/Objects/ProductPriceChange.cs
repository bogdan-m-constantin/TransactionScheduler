namespace TransactionScheduling.Project.Domain.Objects
{
    public class ProductPriceChange(int productId, double price, DateTime timestamp)
    {
        public double ProductId { get; set; } = productId;
        public double Price { get; set; } = price;
        public DateTime Timestamp { get; set; } = timestamp;
    }

}
