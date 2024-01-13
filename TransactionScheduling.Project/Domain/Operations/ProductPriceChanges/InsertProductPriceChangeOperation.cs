using Microsoft.Data.SqlClient;
using System.Data;
using TransactionScheduling.Project.Domain.Objects;
namespace TransactionScheduling.Project.Domain.Operations.ProductPriceChanges
{
    public class InsertProductPriceChangeOperation(SqlConnection con, Product product) : BaseSqlOperation(con)
    {

        public override string TableName => "ProductPriceChanges";

        public override object? Execute()
        {
            RollbackOperation = RollbackOperation.Delete;
            
            base.Execute();

            using var cmd = new SqlCommand($"INSERT INTO ProductPriceChanges (Product,Price) VALUES (@ProductId,@Price); SET @id = SCOPE_IDENTITY()", _con);
            cmd.Parameters.Add(new("@Id", 0) { Direction = ParameterDirection.Output });
            cmd.Parameters.Add(new("@ProductId", product.Id));
            cmd.Parameters.Add(new("@Price", product.Price));

            cmd.ExecuteNonQuery();
            RowIds.Add(Convert.ToInt32(cmd.Parameters["@Id"].Value));

            return null;

        }
    }

}
