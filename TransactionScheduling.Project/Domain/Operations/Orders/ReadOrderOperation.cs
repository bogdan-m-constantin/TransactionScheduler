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
            using var cmd = new SqlCommand($"SELECT * FROM Orders WHERE ClientId = {clientId} {(id == null ? "" : $" AND Id = {id.Value}")}", _con);
            using var reader = cmd.ExecuteReader();
            var lst = new List<Order>();
            while (reader.Read())
            {
                lst.Add(new Order(
                    Convert.ToInt32(reader["Id"]),
                    Convert.ToInt32(reader["Client"]),
                    Convert.ToDateTime(reader["Timestamp"]),
                    []
                    )
                );
            }

            return lst;
        }
    }

}
