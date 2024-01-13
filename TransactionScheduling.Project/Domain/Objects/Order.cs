namespace TransactionScheduling.Project.Domain.Objects
{
    public class Order(int id,int client, DateTime timestamp, List<OrderItem> items, double total) : BaseEntity(id)
    {
        public int Client { get; set; } = client;
        public DateTime Timestamp { get; set; } = timestamp;
        public List<OrderItem> Items { get; set; } = items;
        public double Total { get; set; } = total;
        

    }
    
}
