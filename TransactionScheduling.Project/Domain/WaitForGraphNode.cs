namespace TransactionScheduling.Project.Domain
{
    public class WaitForGraphNode
    {
        public Guid Id { get; init; } = Guid.NewGuid();
         public Table? LockTable { get; set; }
        public Transaction? Transaction { get; set; }
        public WaitForGraphNode? TransactionWaitingLock { get; set; }

        public void SetWaitingLock(WaitForGraphNode node)
        {
            if (TransactionWaitingLock == null)
                TransactionWaitingLock = node;
            else
                TransactionWaitingLock.SetWaitingLock(node);
        }

    }
}
