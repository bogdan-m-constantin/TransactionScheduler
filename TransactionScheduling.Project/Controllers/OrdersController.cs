using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using TransactionScheduling.Project.Domain;
using TransactionScheduling.Project.Domain.Objects;
using TransactionScheduling.Project.Domain.Operations;
using TransactionScheduling.Project.Domain.Operations.Clients;
using TransactionScheduling.Project.Domain.Operations.OrderItems;
using TransactionScheduling.Project.Domain.Operations.Orders;
using TransactionScheduling.Project.Domain.Operations.Products;
using TransactionScheduling.Project.Domain.Operations.ProductStockChanges;
using TransactionScheduling.Project.Domain.SQL;
using TransactionScheduling.Project.Services;

namespace TransactionScheduling.Project
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController(TransactionSchedulerService service, SqlOptions sql) : ControllerBase
    {

        private readonly TransactionSchedulerService _service = service;
        private readonly SqlOptions _sql = sql;

        [HttpGet("{clientId}")]
        public ActionResult<List<Order>> GetOrders(int clientId)
        {
            using var con = new SqlConnection(_sql[SqlDatabase.DB2]);
            con.Open();
            var transaction = new Transaction();
            transaction.Operations.Enqueue(new ReadOrderOperation(con, clientId, null));
            var resp = _service.Run(transaction);
            return Ok((List<Order>)resp[0]!);
        }
        [HttpGet("items/{orderId}")]
        public ActionResult<List<OrderItem>> GetOrderItems(int orderId)
        {
            using var con = new SqlConnection(_sql[SqlDatabase.DB2]);
            con.Open();
            var transaction = new Transaction();
            transaction.Operations.Enqueue(new ReadOrderItemsOperation(con, orderId));
            var resp = _service.Run(transaction);
            return Ok((List<OrderItem>)resp[0]!);
        }
        [HttpPost()]
        public ActionResult<Order> InsertOrder([FromBody] Order order)
        {
            using var con1 = new SqlConnection(_sql[SqlDatabase.DB1]);
            con1.Open();
            using var con2 = new SqlConnection(_sql[SqlDatabase.DB2]);
            con2.Open();
            var transaction = new Transaction();
            transaction.Operations.Enqueue(new InsertOrderOperation(con2, order));
            foreach (var item in order.Items)
            {
                transaction.Operations.Enqueue(new InsertOrderItemOperation(con2, item, order));
                transaction.Operations.Enqueue(new RemoveProductStockOperation(con1, item.ProductId, item.Quantity));
                transaction.Operations.Enqueue(new InsertProductStockChangeOperation(con2, new Product(item.ProductId,"","",item.Quantity,0.0),true));
            }
            transaction.Operations.Enqueue(new AddClientPontsOperation(con1, order.Client, (int)order.Items.Sum(e => e.Price * e.Quantity)));
            var resp = _service.Run(transaction);
            return Ok((Order)resp[0]!);
        }
    }
}