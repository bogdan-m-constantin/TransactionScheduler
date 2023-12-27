using Microsoft.Data.SqlClient;
using TransactionScheduling.Project.Domain.Objects;
namespace TransactionScheduling.Project.Domain.Operations.Orders
{
    public class ReadOrderOperation(SqlConnection con, int? id) : BaseSqlOperation<List<Order>>(con)
    {
        public override string TableName => "Orders";

        public override List<Order> Execute()
        {
            RollbackOperation = RollbackOperation.Nothing;

            base.Execute();
            using var cmd = new SqlCommand($"SELECT * FROM Orders {(id == null ? "" : $"WHERE Id = {id.Value}")}", _con);
            using var reader = cmd.ExecuteReader();
            var lst = new List<Order>();
            while (reader.Read())
            {
                lst.Add(new Order(
                    Convert.ToInt32(reader["Id"]),
                    Convert.ToInt32(reader["Client"]),
                    Convert.ToDateTime(reader["Timestamp"]),
                    new()
                    )
                );
            }

            return lst;
        }
    }

}
