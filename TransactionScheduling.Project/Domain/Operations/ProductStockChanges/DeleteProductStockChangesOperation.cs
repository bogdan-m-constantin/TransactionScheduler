using Microsoft.Data.SqlClient;
using System.Data;
using TransactionScheduling.Project.Domain.Objects;
namespace TransactionScheduling.Project.Domain.Operations.ProductStockChanges
{
    public class DeleteProductStockChangesOperation(SqlConnection con, int productId) : BaseSqlOperation(con)
    {
        public override string TableName => "ProductStockChanges";

        public override object? Execute()
        {
            RollbackOperation = RollbackOperation.Insert;

            using var preCmd = new SqlCommand($"SELECT Id FROM ProductStockChanges WHERE Product = @Id", _con);
            preCmd.Parameters.Add(new("@Id", productId));
            using var reader = preCmd.ExecuteReader();
            while(reader.Read())
                RowIds.Add(reader.GetInt32(0));

            reader.Close();
            base.Execute();

            using var cmd = new SqlCommand($"DELETE FROM ProductStockChanges WHERE Product = @Id", _con);
            cmd.Parameters.Add(new("@Id", productId));
            cmd.ExecuteNonQuery();

            return null;

        }
    }

}
