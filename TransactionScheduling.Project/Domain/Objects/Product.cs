namespace TransactionScheduling.Project.Domain.Objects
{
    public class Product(int id, string name, string description, int stock, double price) : BaseEntity(id)
    {
        public string Name { get; set; } = name;
        public string Description { get; set; } = description;
        public int Stock { get; set; } = stock;
        public double Price { get; set; } = price;
    }
    
}
