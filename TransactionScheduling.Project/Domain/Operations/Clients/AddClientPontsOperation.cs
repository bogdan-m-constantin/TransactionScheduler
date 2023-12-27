using Microsoft.Data.SqlClient;
namespace TransactionScheduling.Project.Domain.Operations.Clients
{
    public class AddClientPontsOperation(SqlConnection con, int id, int points) : BaseSqlOperation(con: con)
    {
        public override string TableName => "Clients";

        public override object? Execute()
        {
            RollbackOperation = RollbackOperation.Update;
            RowId = id;
            base.Execute();

            using var cmd = new SqlCommand($"UPDATE Clients SET  AmmountOfPoints = AmmountOfPoints + @AmmountOfPoints WHERE Id = @Id", _con);
            cmd.Parameters.Add(new("@Id", id));
            cmd.Parameters.Add(new("@AmmountOfPoints", points));
            cmd.ExecuteNonQuery();
            return null;
        }
    }

}
    