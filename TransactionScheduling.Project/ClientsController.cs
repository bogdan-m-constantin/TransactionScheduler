using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using TransactionScheduling.Project.Domain;
using TransactionScheduling.Project.Domain.Objects;
using TransactionScheduling.Project.Domain.Operations;
using TransactionScheduling.Project.Domain.Operations.Clients;
using TransactionScheduling.Project.Domain.SQL;
using TransactionScheduling.Project.Services;

[ApiController]
[Route("[controller]")]
public class ClientsController : ControllerBase {

    private readonly TransactionSchedulerService _service;
    private readonly SqlOptions _sql;

    public ClientsController(TransactionSchedulerService service, SqlOptions sql)
    {
        _service = service;
        _sql = sql;
    }

    [HttpGet]
    public ActionResult<List<Client>> GetClients()
    {
        using var con1 = new SqlConnection(_sql[SqlDatabase.DB1]);
        con1.Open();
        _service.Run(new Transaction()
        {
            Operations = new BaseSqlOperation<object>[]
            {
                new ReadClientOperation(con1,null)
            }
        });
        return new();


    }
}