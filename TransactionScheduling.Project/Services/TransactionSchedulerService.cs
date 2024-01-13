using Azure;
using Microsoft.Identity.Client;
using System.Net.Http.Headers;

using TransactionScheduling.Project.Domain;

namespace TransactionScheduling.Project.Services
{
    public class TransactionSchedulerService(WaitForGraph graph, ILogger<TransactionSchedulerService> logger)
    {
        private readonly ILogger<TransactionSchedulerService> _logger = logger;
        private readonly WaitForGraph _graph = graph;


        public List<object?> Run(Transaction transaction)
        {
            CancellationTokenSource tokenSource = new();
            transaction.CancelationToken = tokenSource;
            var response = new List<object?>();
            _logger.LogInformation($"Incep tranzactie {transaction.Id}");
            ExecuteLocking(transaction.TablesToLock, transaction, 0, tokenSource.Token, response);
            return response;

        }

        private void ExecuteLocking(List<string> tablesToLock, Transaction t,  int index, CancellationToken token, List<object?> response)
        {

            token.ThrowIfCancellationRequested();
            if (index >= tablesToLock.Count)
            {
                try
                {
                    foreach (var op in t.Operations)
                    {
                        token.ThrowIfCancellationRequested();

                        var res = op.Execute();
                        response.Add(res);

                        t.SuccessfullOperations++;
                    }
                    _logger.LogInformation($"Finalizat tranzactie {t.Id} cu succes (COMMIT)");
                    return;
                }
                catch (Exception ex)
                {
                    response.Clear();
                    _logger.LogError("Incep rollback tranzactie. Cauza: " + ex.Message);
                    foreach (var op in t.Operations.Take(t.SuccessfullOperations).Reverse())
                    {
                        op.Rollback();
                    }
                    _logger.LogInformation($"Finalizat rollback {t.Id} (ABORT)");
                    throw;

                }
            }

            _logger.LogInformation($"{t.Id} Astept sa primesc lock pe tabelul {tablesToLock[index]}");
            var node = _graph.AquireLock(tablesToLock[index], t);
            lock (node.LockTable!.LockObj)
            {
                _logger.LogInformation($"{t.Id} Am primit lock pe tabelul {node.LockTable.Name} {node.LockTable.LockObj.GetHashCode()}");
                try
                {
                    //Thread.Sleep(1000); -- pentru testat locking. 
                    ExecuteLocking(tablesToLock, t, index + 1, token, response);
                }
                finally
                {
                    WaitForGraph.Release(node);
                }
            }


        }

    }
}
