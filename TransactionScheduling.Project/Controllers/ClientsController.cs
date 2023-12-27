using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using TransactionScheduling.Project.Domain;
using TransactionScheduling.Project.Domain.Objects;
using TransactionScheduling.Project.Domain.Operations;
using TransactionScheduling.Project.Domain.Operations.Clients;
using TransactionScheduling.Project.Domain.SQL;
using TransactionScheduling.Project.Services;

namespace TransactionScheduling.Project.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientsController(TransactionSchedulerService service, SqlOptions sql) : ControllerBase
    {

        private readonly TransactionSchedulerService _service = service;
        private readonly SqlOptions _sql = sql;
        [HttpGet("")]
        public ActionResult<List<Client>> GetClients()
        {
            using var con1 = new SqlConnection(_sql[SqlDatabase.DB1]);
            con1.Open();
            var transaction = new Transaction();
            transaction.Operations.Enqueue(new ReadClientOperation(con1, null));
            var resp = _service.Run(transaction);
            return Ok((List<Client>)resp[0]!);


        }
        [HttpGet("{clientId}")]
        public ActionResult<List<Client>> GetClients(int clientId)
        {
            using var con1 = new SqlConnection(_sql[SqlDatabase.DB1]);
            con1.Open();
            var transaction = new Transaction();
            transaction.Operations.Enqueue(new ReadClientOperation(con1, clientId));
            var resp = _service.Run(transaction);
            return Ok((List<Client>)resp[0]!);


        }
    }
}