using Microsoft.Data.SqlClient;
using TransactionScheduling.Project.Domain.Objects;
namespace TransactionScheduling.Project.Domain.Operations.ProductStockChanges
{
    public class ReadProductStockChangesOperation(SqlConnection con, int productId) : BaseSqlOperation(con)
    {
        public override string TableName => "ProductStockChanges";

        public override List<ProductStockChange> Execute()
        {
            RollbackOperation = RollbackOperation.Nothing;

            base.Execute();
            using var cmd = new SqlCommand($"SELECT * FROM ProductStockChanges WHERE Product = @Product ORDER BY Timestamp", _con);
            cmd.Parameters.Add(new SqlParameter("@Product", productId));
            using var reader = cmd.ExecuteReader();
            var lst = new List<ProductStockChange>();
            while (reader.Read())
            {
                lst.Add(new ProductStockChange(
                    Convert.ToInt32(reader["Product"]),
                    Convert.ToDouble(reader["Stock"]),
                    Convert.ToDateTime(reader["Timestamp"])
                    )
                );
            }

            return lst;
        }
    }

}
