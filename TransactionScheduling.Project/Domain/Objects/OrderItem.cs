namespace TransactionScheduling.Project.Domain.Objects
{
    public class OrderItem(int id, int product, string productName, double price,int quantity) : BaseEntity(id)
    {
        public int ProductId { get; set; } = product;
        public string ProductName { get; set; } = productName;
        public double Price { get; set; } = price;
        public int Quantity { get; set; } = quantity;
    }
    
}
