using Microsoft.Data.SqlClient;
using System.Data;
using TransactionScheduling.Project.Domain.Objects;
namespace TransactionScheduling.Project.Domain.Operations.Products
{
    public class InsertProductOperation(SqlConnection con, Product product) : BaseSqlOperation(con)
    {
        public override string TableName => "Products";

        public override object? Execute()
        {
            RollbackOperation = RollbackOperation.Delete;
            
            base.Execute();

            using var cmd = new SqlCommand($"INSERT INTO Products (Name,Description ,Stock, Price,Image) VALUES (@Name,@Description, @Stock, @Price,@Image); SET @id = SCOPE_IDENTITY()", _con);
            cmd.Parameters.Add(new("@Id", 0) { Direction = ParameterDirection.Output });
            cmd.Parameters.Add(new("@Name", product.Name));
            cmd.Parameters.Add(new("@Description", product.Description));
            cmd.Parameters.Add(new("@Stock", product.Stock));
            cmd.Parameters.Add(new("@Price", product.Price));
            cmd.Parameters.Add(new("@Image", product.Image));
            cmd.ExecuteNonQuery();
            product.Id = Convert.ToInt32(cmd.Parameters["@Id"].Value);

            RowIds.Add(product.Id);
            return product;

        }
    }

}
