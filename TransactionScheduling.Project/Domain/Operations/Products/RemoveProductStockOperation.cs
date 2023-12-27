using Microsoft.Data.SqlClient;
namespace TransactionScheduling.Project.Domain.Operations.Products
{
    public class RemoveProductStockOperation(SqlConnection con, int id, int quantity) : BaseSqlOperation(con: con)
    {
        public override string TableName => "Products";

        public override object? Execute()
        {
            RollbackOperation = RollbackOperation.Update;
            RowId = id;
            base.Execute();

            using var cmd = new SqlCommand($"UPDATE Products SET  Stock = Stock - @Quantity WHERE Id = @Id", _con);
            cmd.Parameters.Add(new("@Id", id));
            cmd.Parameters.Add(new("@Quantity", quantity));
            cmd.ExecuteNonQuery();
            return null;
        }
    }

}
