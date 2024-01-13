using Microsoft.Data.SqlClient;
using TransactionScheduling.Project.Domain.Objects;
namespace TransactionScheduling.Project.Domain.Operations.Orders
{
    public class ReadOrderOperation(SqlConnection con, int clientId, int? id) : BaseSqlOperation(con)
    {
        public override string TableName => "Orders";

        public override List<Order> Execute()
        {
            RollbackOperation = RollbackOperation.Nothing;

            base.Execute();
            using var cmd = new SqlCommand($"SELECT * FROM Orders WHERE (Client = @ClientId OR -1 = @ClientId) {(id == null ? "" : $" AND Id = @Id")}", _con);
            cmd.Parameters.Add(new("@ClientId", clientId));
            cmd.Parameters.Add(new("@Id", id ?? (object)DBNull.Value));
            using var reader = cmd.ExecuteReader();
            var lst = new List<Order>();
            while (reader.Read())
            {
                lst.Add(new Order(
                    Convert.ToInt32(reader["Id"]),
                    Convert.ToInt32(reader["Client"]),
                    Convert.ToDateTime(reader["Timestamp"]),
                    [],
                    Convert.ToDouble(reader["Total"])
                    )
                );
            }

            return lst;
        }
    }

}
