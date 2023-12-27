using Microsoft.Data.SqlClient;
using TransactionScheduling.Project.Domain.Objects;
namespace TransactionScheduling.Project.Domain.Operations.Clients
{
    public class ReadClientOperation(SqlConnection con, int? id) : BaseSqlOperation(con)
    {
        public override string TableName => "Clients";

        public override List<Client> Execute()
        {
            RollbackOperation = RollbackOperation.Nothing;

            base.Execute();
            using var cmd = new SqlCommand($"SELECT * FROM Clients {(id == null ? "" : $"WHERE Id = {id.Value}")}", _con);
            using var reader = cmd.ExecuteReader();
            var lst = new List<Client>();
            while (reader.Read())
            {
                lst.Add(new Client(
                    Convert.ToInt32(reader["Id"]),
                    reader["FirstName"].ToString()!,
                    reader["LastName"].ToString()!,
                    reader["PersonalCode"].ToString()!,
                    reader["IdNumber"].ToString()!,
                    Convert.ToDateTime(reader["DateOfBirth"]),
                    Convert.ToDouble(reader["AmmountOfPoints"])
                    )
                );
            }

            return lst;
        }
    }

}
