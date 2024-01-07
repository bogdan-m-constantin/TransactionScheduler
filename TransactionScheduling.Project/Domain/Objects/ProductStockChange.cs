namespace TransactionScheduling.Project.Domain.Objects
{
    public class ProductStockChange(int productId, double price)
    {
        public double ProductId { get; set; } = productId;
        public double Stock { get; set; } = price;
    }

}
