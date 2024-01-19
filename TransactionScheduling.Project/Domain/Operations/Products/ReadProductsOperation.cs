using Microsoft.Data.SqlClient;
using TransactionScheduling.Project.Domain.Objects;
namespace TransactionScheduling.Project.Domain.Operations.Products
{
    public class ReadProductsOperation(SqlConnection con, int? id) : BaseSqlOperation(con)
    {
        public override string TableName => "Products";

        public override List<Product> Execute()
        {
            RollbackOperation = RollbackOperation.Nothing;

            base.Execute();
            using var cmd = new SqlCommand($"SELECT * FROM Products {(id == null ? "" : $" WHERE Id = {id.Value}")}", _con);
            using var reader = cmd.ExecuteReader();
            var lst = new List<Product>();
            while (reader.Read())
            {
                lst.Add(new Product(
                    Convert.ToInt32(reader["Id"]),
                    reader["Name"].ToString()!,
                    reader["Description"].ToString()!,
                    Convert.ToInt32(reader["Stock"]),
                    Convert.ToDouble(reader["Price"]),
                    reader["Image"].ToString()!
                    )
                );
            }

            return lst;
        }
    }

}
