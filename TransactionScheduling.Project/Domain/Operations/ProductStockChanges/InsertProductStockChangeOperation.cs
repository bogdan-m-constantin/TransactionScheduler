using Microsoft.Data.SqlClient;
using System.Data;
using TransactionScheduling.Project.Domain.Objects;
namespace TransactionScheduling.Project.Domain.Operations.ProductStockChanges
{
    public class InsertProductStockChangeOperation(SqlConnection con,Product product, bool modifier) : BaseSqlOperation(con)
    {
        public override string TableName => "ProductStockChanges";

        public override object? Execute()
        {
            RollbackOperation = RollbackOperation.Delete;

            base.Execute();

            if (modifier)
            {
                using var cmd = new SqlCommand($"INSERT INTO ProductStockChanges (Product,Stock) SELECT TOP 1 Product,Stock + @Stock FROM ProductStockChanges WHERE Product = @ProductId ORDER BY Id desc; SET @id = SCOPE_IDENTITY()", _con);
                cmd.Parameters.Add(new("@Id", 0) { Direction = ParameterDirection.Output });
                cmd.Parameters.Add(new("@ProductId", product.Id));
                cmd.Parameters.Add(new("@Stock", product.Stock));

                cmd.ExecuteNonQuery();
                RowIds.Add(Convert.ToInt32(cmd.Parameters["@Id"].Value));
            }
            else
            {
                using var cmd = new SqlCommand($"INSERT INTO ProductStockChanges (Product,Stock) VALUES (@ProductId,@Stock); SET @id = SCOPE_IDENTITY()", _con);
                cmd.Parameters.Add(new("@Id", 0) { Direction = ParameterDirection.Output });
                cmd.Parameters.Add(new("@ProductId", product.Id));
                cmd.Parameters.Add(new("@Stock", product.Stock));

                cmd.ExecuteNonQuery();
                RowIds.Add(Convert.ToInt32(cmd.Parameters["@Id"].Value));
            }
            return null;

        }
    }

}
