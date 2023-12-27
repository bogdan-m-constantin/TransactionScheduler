using Microsoft.Data.SqlClient;
using System.Data;
using TransactionScheduling.Project.Domain.Objects;
namespace TransactionScheduling.Project.Domain.Operations.Clients
{
    public class InsertClientOperation(SqlConnection con, Client client) : BaseSqlOperation<object?>(con)
    {
        public override string TableName => "Clients";

        public override object? Execute()
        {
            RollbackOperation = RollbackOperation.Delete;
            RowId = client.Id;
            base.Execute();

            using var cmd = new SqlCommand($"INSERT INTO Clients (FirstName ,LastName , PersonalCode , IdNumber , DateOfBirth , AmmountOfPoints) VALUES (@FirstName ,@LastName , @PersonalCode , @IdNumber , @DateOfBirth , @AmmountOfPoints); SET @id = SCOPE_IDENTITY()", _con);
            cmd.Parameters.Add(new("@Id", 0) { Direction = ParameterDirection.Output });
            cmd.Parameters.Add(new("@FirstName", client.FirstName));
            cmd.Parameters.Add(new("@LastName", client.LastName));
            cmd.Parameters.Add(new("@PersonalCode", client.PersonalCode));
            cmd.Parameters.Add(new("@IdNumber", client.IdNumber));
            cmd.Parameters.Add(new("@DateOfBirth", client.DateOfBirth));
            cmd.Parameters.Add(new("@AmmountOfPoints", client.AmmountOfPoints));
            cmd.ExecuteNonQuery();
            client.Id = Convert.ToInt32(cmd.Parameters["@Id"]);
            return client;

        }
    }

}
