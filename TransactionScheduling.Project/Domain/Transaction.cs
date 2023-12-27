using Microsoft.OpenApi.Any;
using TransactionScheduling.Project.Domain.Operations;

namespace TransactionScheduling.Project.Domain
{
    // a list of operations which need to be executed in an all-or-none fashion.
    public class Transaction
    {

        public CancellationTokenSource? CancelationToken { get; set; }
        public Guid Id { get; } = Guid.NewGuid();
        public int SuccessfullOperations;
        public Queue<BaseSqlOperation<object>> Operations { get; set; } = new();
        public List<string> TablesToLock => Operations.Select(o => o.TableName).Distinct().ToList();
        
    }
    
}

