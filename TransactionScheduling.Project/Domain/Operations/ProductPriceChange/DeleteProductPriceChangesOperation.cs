using Microsoft.Data.SqlClient;
using System.Data;
using TransactionScheduling.Project.Domain.Objects;
namespace TransactionScheduling.Project.Domain.Operations.ProductPriceChange
{
    public class DeleteProductPriceChangesOperation(SqlConnection con, int productId) : BaseSqlOperation(con)
    {
        public override string TableName => "ProductPriceChanges";

        public override object? Execute()
        {
            RollbackOperation = RollbackOperation.Insert;

            using var preCmd = new SqlCommand($"SELECT Id FROM ProductPriceChanges WHERE Product = @Id", _con);

            preCmd.Parameters.Add(new("@Id", productId));
            using var reader = preCmd.ExecuteReader();
            while (reader.Read())
                RowIds.Add(reader.GetInt32(0));

            base.Execute();

            using var cmd = new SqlCommand($"DELETE FROM ProductPriceChanges WHERE Product = @Id", _con);
            cmd.Parameters.Add(new("@Id", productId));
            cmd.ExecuteNonQuery();

            return null;

        }
    }

}
