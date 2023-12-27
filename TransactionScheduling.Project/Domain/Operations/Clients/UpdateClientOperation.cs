using Microsoft.Data.SqlClient;
using TransactionScheduling.Project.Domain.Objects;
namespace TransactionScheduling.Project.Domain.Operations.Clients
{
    public class UpdateClientOperation(SqlConnection con, Client client) : BaseSqlOperation(con)
    {
        public override string TableName => "Clients";

        public override object? Execute()
        {
            RollbackOperation = RollbackOperation.Update;
            RowId = client.Id;
            base.Execute();

            using var cmd = new SqlCommand($"UPDATE Clients SET  FirstName = @FirstName,LastName = @LastName, PersonalCode = @PersonalCode, IdNumber = @IdNumber, DateOfBirth = @DateOfBirth, AmmountOfPoints = @AmmountOfPoints WHERE Id = @Id", _con);
            cmd.Parameters.Add(new("@Id", client.Id));
            cmd.Parameters.Add(new("@FirstName", client.FirstName));
            cmd.Parameters.Add(new("@LastName", client.LastName));
            cmd.Parameters.Add(new("@PersonalCode", client.PersonalCode));
            cmd.Parameters.Add(new("@IdNumber", client.IdNumber));
            cmd.Parameters.Add(new("@DateOfBirth", client.DateOfBirth));
            cmd.Parameters.Add(new("@AmmountOfPoints", client.AmmountOfPoints));
            cmd.ExecuteNonQuery();
            return client;

        }
    }

}
