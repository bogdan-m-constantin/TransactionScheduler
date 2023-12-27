using System.Text.Json.Serialization;

namespace TransactionScheduling.Project.Domain.Objects
{
    public class OrderItem(int id, int productId, string productName, double price,int quantity)

    { 
        public int Id { get; set; } = id;
        public int ProductId { get; set; } = productId;
        public string ProductName { get; set; } = productName;
        public double Price { get; set; } = price;
        public int Quantity { get; set; } = quantity;
    }
    
}
