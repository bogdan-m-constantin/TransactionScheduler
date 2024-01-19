using Microsoft.Data.SqlClient;
using System.Data;
using TransactionScheduling.Project.Domain.Objects;
namespace TransactionScheduling.Project.Domain.Operations.Products
{
    public class UpdateProductOperation(SqlConnection con, Product product) : BaseSqlOperation(con)
    {
        public override string TableName => "Products";

        public override object? Execute()
        {
            RollbackOperation = RollbackOperation.Delete;
            RowIds.Add(product.Id);
            base.Execute();
            
            using var cmd = new SqlCommand($"UPDATE Products SET Name = @Name,Description = @Description,Stock = @Stock, Price = @Price,Image=@Image WHERE Id = @Id", _con);
            cmd.Parameters.Add(new("@Id", product.Id));
            cmd.Parameters.Add(new("@Name", product.Name));
            cmd.Parameters.Add(new("@Description", product.Description));
            cmd.Parameters.Add(new("@Stock", product.Stock));
            cmd.Parameters.Add(new("@Price", product.Price));
            cmd.Parameters.Add(new("@Image", product.Image));
            cmd.ExecuteNonQuery();
            return product;

        }
    }

}
