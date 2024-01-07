using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using TransactionScheduling.Project.Domain;
using TransactionScheduling.Project.Domain.Objects;
using TransactionScheduling.Project.Domain.Operations;
using TransactionScheduling.Project.Domain.Operations.Clients;
using TransactionScheduling.Project.Domain.Operations.OrderItems;
using TransactionScheduling.Project.Domain.Operations.Orders;
using TransactionScheduling.Project.Domain.Operations.ProductPriceChange;
using TransactionScheduling.Project.Domain.Operations.Products;
using TransactionScheduling.Project.Domain.Operations.ProductStockChange;
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
        [HttpPost()]
        public ActionResult<Order> InsertProduct([FromBody] Product product)
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
            return Ok((Order)resp[0]!);
        }

        [HttpPut()]
        public ActionResult<Order> UpdateProduct([FromBody] Product product)
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
            return Ok((Order)resp[0]!);
        }

        [HttpDelete("{product}")]
        public ActionResult<Order> UpdateProduct( int product)
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
            return Ok((Order)resp[0]!);
        }
    }
}