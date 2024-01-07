using Microsoft.Data.SqlClient;
using System.Data;
using TransactionScheduling.Project.Domain.Objects;
namespace TransactionScheduling.Project.Domain.Operations.Products
{
    public class DeleteProductOperation(SqlConnection con, int id) : BaseSqlOperation(con)
    {
        public override string TableName => "Products";

        public override object? Execute()
        {
            RollbackOperation = RollbackOperation.Insert;
            RowIds.Add(id);
            base.Execute();

            using var cmd = new SqlCommand($"DELETE FROM Products WHERE Id = @Id", _con);
            cmd.Parameters.Add(new("@Id", id));
            cmd.ExecuteNonQuery();

            return null;

        }
    }

}
