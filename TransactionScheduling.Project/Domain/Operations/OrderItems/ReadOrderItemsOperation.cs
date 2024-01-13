using Microsoft.Data.SqlClient;
using TransactionScheduling.Project.Domain.Objects;
namespace TransactionScheduling.Project.Domain.Operations.OrderItems
{
    public class ReadOrderItemsOperation(SqlConnection con, int? id) : BaseSqlOperation(con)
    {
        public override string TableName => "OrderItems";

        public override List<OrderItem> Execute()
        {
            RollbackOperation = RollbackOperation.Nothing;

            base.Execute();
            using var cmd = new SqlCommand($"SELECT * FROM OrderItems {(id == null ? "" : $"WHERE OrderId = {id.Value}")}", _con);
            using var reader = cmd.ExecuteReader();
            var lst = new List<OrderItem>();
            while (reader.Read())
            {
                lst.Add(new OrderItem(
                    Convert.ToInt32(reader["Id"]),
                    Convert.ToInt32(reader["ProductId"]),
                    reader["ProductName"].ToString(),
                    Convert.ToDouble(reader["Price"]),
                    Convert.ToInt32(reader["Quantity"])
                    )
                );
            }

            return lst;
        }
    }

}
