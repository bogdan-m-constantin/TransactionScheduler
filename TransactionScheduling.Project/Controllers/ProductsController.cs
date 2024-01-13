using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using TransactionScheduling.Project.Domain;
using TransactionScheduling.Project.Domain.Objects;
using TransactionScheduling.Project.Domain.Operations;
using TransactionScheduling.Project.Domain.Operations.Clients;
using TransactionScheduling.Project.Domain.Operations.OrderItems;
using TransactionScheduling.Project.Domain.Operations.Orders;
using TransactionScheduling.Project.Domain.Operations.ProductPriceChanges;
using TransactionScheduling.Project.Domain.Operations.Products;
using TransactionScheduling.Project.Domain.Operations.ProductStockChanges;
using TransactionScheduling.Project.Domain.SQL;
using TransactionScheduling.Project.Services;

namespace TransactionScheduling.Project
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController(TransactionSchedulerService service, SqlOptions sql) : ControllerBase
    {

        private readonly TransactionSchedulerService _service = service;
        private readonly SqlOptions _sql = sql;

        [HttpGet("")]
        public ActionResult<List<Product>> GetProducts()
        {
            using var con1 = new SqlConnection(_sql[SqlDatabase.DB1]);
            con1.Open();
            var transaction = new Transaction();
            transaction.Operations.Enqueue(new ReadProductsOperation(con1,null));
            var resp = _service.Run(transaction);
            return Ok((List<Product>)resp[0]!);
        }
        [HttpGet("price-changes/{product}")]
        public ActionResult<List<ProductPriceChange>> GetPriceChanges(int product)
        {
            using var con = new SqlConnection(_sql[SqlDatabase.DB2]);
            con.Open();
            var transaction = new Transaction();
            transaction.Operations.Enqueue(new ReadProductPriceChangesOperation(con, product));
            var resp = _service.Run(transaction);
            return Ok((List<ProductPriceChange>)resp[0]!);
        }

        [HttpGet("stock-changes/{product}")]
        public ActionResult<List<ProductStockChange>> GetStockChanges(int product)
        {
            using var con = new SqlConnection(_sql[SqlDatabase.DB2]);
            con.Open();
            var transaction = new Transaction();
            transaction.Operations.Enqueue(new ReadProductStockChangesOperation(con, product));
            var resp = _service.Run(transaction);
            return Ok((List<ProductStockChange>)resp[0]!);
        }
        [HttpPost()]
        public ActionResult<Product> InsertProduct([FromBody] Product product)
        {
            using var con1 = new SqlConnection(_sql[SqlDatabase.DB1]);
            con1.Open();
            using var con2 = new SqlConnection(_sql[SqlDatabase.DB2]);
            con2.Open();
            var transaction = new Transaction();
            transaction.Operations.Enqueue(new InsertProductOperation(con1, product));
            transaction.Operations.Enqueue(new InsertProductPriceChangeOperation(con2, product));
            transaction.Operations.Enqueue(new InsertProductStockChangeOperation(con2, product, false));

            var resp = _service.Run(transaction);
            return Ok((Product)resp[0]!);
        }

        [HttpPut()]
        public ActionResult<Product> UpdateProduct([FromBody] Product product)
        {
            using var con1 = new SqlConnection(_sql[SqlDatabase.DB1]);
            con1.Open();
            using var con2 = new SqlConnection(_sql[SqlDatabase.DB2]);
            con2.Open();
            var transaction = new Transaction();
            transaction.Operations.Enqueue(new UpdateProductOperation(con1, product));
            transaction.Operations.Enqueue(new InsertProductPriceChangeOperation(con2, product));
            transaction.Operations.Enqueue(new InsertProductStockChangeOperation(con2, product, false));

            var resp = _service.Run(transaction);
            return Ok((Product)resp[0]!);
        }

        [HttpDelete("{product}")]
        public ActionResult<Product> DeleteProduct( int product)
        {
            using var con1 = new SqlConnection(_sql[SqlDatabase.DB1]);
            con1.Open();
            using var con2 = new SqlConnection(_sql[SqlDatabase.DB2]);
            con2.Open();
            var transaction = new Transaction();
            transaction.Operations.Enqueue(new DeleteProductPriceChangesOperation(con2, product));
            transaction.Operations.Enqueue(new DeleteProductStockChangesOperation(con2, product));
            transaction.Operations.Enqueue(new DeleteProductOperation(con1, product));

            var resp = _service.Run(transaction);
            return Accepted();
        }

    }
}