namespace TransactionScheduling.Project.Domain.Objects
{
    public class ProductPriceChange(int productId, double price)
    {
        public double ProductId { get; set; } = productId;
        public double Price { get; set; } = price;
    }

}
