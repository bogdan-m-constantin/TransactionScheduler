using Microsoft.Data.SqlClient;
using System.Data;
using TransactionScheduling.Project.Domain.Objects;
namespace TransactionScheduling.Project.Domain.Operations.Orders
{
    public class InsertOrderOperation(SqlConnection con, Order order) : BaseSqlOperation(con)
    {
        public override string TableName => "Orders";

        public override object? Execute()
        {
            RollbackOperation = RollbackOperation.Delete;
            base.Execute();

            using var cmd = new SqlCommand($"INSERT INTO Orders (Client ,Timestamp) VALUES (@Client ,@Timestamp); SET @id = SCOPE_IDENTITY()", _con);
            cmd.Parameters.Add(new("@Id", 0) { Direction = ParameterDirection.Output });
            cmd.Parameters.Add(new("@Client", order.Client));
            cmd.Parameters.Add(new("@Timestamp", order.Timestamp));
            cmd.ExecuteNonQuery();
            order.Id = Convert.ToInt32(cmd.Parameters["@Id"].Value);
            RowIds.Add(order.Id);
            return order;

        }
    }

}
