using Microsoft.Data.SqlClient;
using System.Data;
using TransactionScheduling.Project.Domain.Objects;
namespace TransactionScheduling.Project.Domain.Operations.OrderItems
{
    public class InsertOrderItemOperation(SqlConnection con, OrderItem item, Order order) : BaseSqlOperation(con)
    {
        public override string TableName => "OrderItems";

        public override object? Execute()
        {
            RollbackOperation = RollbackOperation.Delete;
            
            base.Execute();

            using var cmd = new SqlCommand($"INSERT INTO OrderItems (ProductId ,ProductName, Price, Quantity, OrderId) VALUES (@ProductId ,@ProductName, @Price, @Quantity, @OrderId); SET @id = SCOPE_IDENTITY()", _con);
            cmd.Parameters.Add(new("@Id", 0) { Direction = ParameterDirection.Output });
            cmd.Parameters.Add(new("@ProductId", item.ProductId));
            cmd.Parameters.Add(new("@ProductName", item.ProductName));
            cmd.Parameters.Add(new("@Price", item.Price));
            cmd.Parameters.Add(new("@Quantity", item.Quantity));
            cmd.Parameters.Add(new("@OrderId", order.Id));
            cmd.ExecuteNonQuery();
            item.Id = Convert.ToInt32(cmd.Parameters["@Id"].Value);
            RowId = item.Id;
            return item;
        }
    }

}
