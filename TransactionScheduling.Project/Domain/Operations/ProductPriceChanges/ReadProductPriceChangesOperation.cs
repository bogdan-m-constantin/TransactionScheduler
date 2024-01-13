using Microsoft.Data.SqlClient;
using TransactionScheduling.Project.Domain.Objects;
namespace TransactionScheduling.Project.Domain.Operations.ProductPriceChanges
{
    public class ReadProductPriceChangesOperation(SqlConnection con, int productId) : BaseSqlOperation(con)
    {
        public override string TableName => "ProductPriceChanges";

        public override List<ProductPriceChange> Execute()
        {
            RollbackOperation = RollbackOperation.Nothing;

            base.Execute();
            using var cmd = new SqlCommand($"SELECT * FROM ProductPriceChanges WHERE Product = @Product ORDER BY Timestamp", _con);
            cmd.Parameters.Add(new SqlParameter("@Product", productId));
            using var reader = cmd.ExecuteReader();
            var lst = new List<ProductPriceChange>();
            while (reader.Read())
            {
                lst.Add(new ProductPriceChange(
                    Convert.ToInt32(reader["Product"]),
                    Convert.ToDouble(reader["Price"]),
                    Convert.ToDateTime(reader["Timestamp"])
                    )
                );
            }

            return lst;
        }
    }

}
