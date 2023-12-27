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
            try
            {

                _logger.LogInformation($"Incep tranzactie {transaction.Id}");
                ExecuteLocking(transaction.TablesToLock, transaction, () =>
                {

                    foreach (var op in transaction.Operations)
                    {
                        tokenSource.Token.ThrowIfCancellationRequested();
                        
                        var res = op.Execute();
                        response.Add(res);
                     
                        transaction.SuccessfullOperations++;
                    }
                },0, tokenSource.Token);
                _logger.LogInformation($"Finalizat tranzactie {transaction.Id} cu succes (COMMIT)");
            }
            catch (Exception ex)
            {
                response.Clear();
                _logger.LogError(ex, "Incep rollback tranzactie");
                foreach (var op in transaction.Operations.Take(transaction.SuccessfullOperations).Reverse())
                {
                    op.Rollback();
                }
                _logger.LogInformation($"Finalizat rollback {transaction.Id} (ABORT)");
                throw;
            }
            return response;

        }

        private void ExecuteLocking(List<string> tablesToLock,  Transaction t, Action action, int index , CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            if (index >= tablesToLock.Count)
            {
                action();
                return;
            }
            var node = _graph.AquireLock(tablesToLock[index], t);
            lock (node.LockTable!.LockObj)
            {
                try
                {
                    ExecuteLocking(tablesToLock,  t, action, index + 1, token);
                }
                finally
                {
                    WaitForGraph.Release(node);
                }
            }

        }

    }
}
