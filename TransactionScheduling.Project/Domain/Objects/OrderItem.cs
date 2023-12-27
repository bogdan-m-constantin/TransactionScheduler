namespace TransactionScheduling.Project.Domain.Objects
{
    public class OrderItem(int id, Product product, double price,int quantity) : BaseEntity(id)
    {
        public Product Product { get; set; } = product;
        public double Price { get; set; } = price;
        public int Quantity { get; set; } = quantity;
    }
    
}
