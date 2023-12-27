namespace TransactionScheduling.Project.Domain.Objects
{
    public class Order(int id,int client, DateTime timestamp, List<OrderItem> items) : BaseEntity(id)
    {
        public int Client { get; set; } = client;
        public DateTime Timestamp { get; set; } = timestamp;
        public List<OrderItem> Items { get; set; } = items;
    }
    
}
