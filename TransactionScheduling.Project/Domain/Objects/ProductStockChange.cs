namespace TransactionScheduling.Project.Domain.Objects
{
    public class ProductStockChange(int productId, double price, DateTime timestamp)
    {
        public double ProductId { get; set; } = productId;
        public double Stock { get; set; } = price;
        public DateTime Timestamp { get; set; } = timestamp;
    }

}
