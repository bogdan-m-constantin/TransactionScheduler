namespace TransactionScheduling.Project.Domain
{
    public class Table(string name)
    {
        public object LockObj { get; set; } = new object();
        public string Name { get; set; } = name;
        public WaitForGraphNode? TransactionHasLock { get; set; }

        public WaitForGraphNode AquireLock(Transaction transaction)
        {
            if (TransactionHasLock == null)
            {
                TransactionHasLock = new WaitForGraphNode
                {
                    LockTable = this,
                    Transaction = transaction,
                    TransactionWaitingLock = null
                };
                return TransactionHasLock;
            }
            else
            {
                return AquireLockAtTheEnd(transaction, TransactionHasLock);
            }
        }

        private WaitForGraphNode AquireLockAtTheEnd(Transaction transaction, WaitForGraphNode node)
        {
            if (node.TransactionWaitingLock == null)
            {
                node.TransactionWaitingLock = new WaitForGraphNode
                {
                    LockTable = this,
                    Transaction = transaction,
                    TransactionWaitingLock = null
                };
                return node.TransactionWaitingLock;
            }
            else
                return AquireLockAtTheEnd(transaction, node.TransactionWaitingLock);
        }

        internal void Release(WaitForGraphNode node)
        {
            if (TransactionHasLock == null)
                throw new Exception("Invalid state of the system!!!!!");
            TransactionHasLock = ReleaseRecursively(node, TransactionHasLock);
        }

        private static WaitForGraphNode? ReleaseRecursively(WaitForGraphNode toRelase, WaitForGraphNode? current)
        {
            if (current == null)
                return null;
            if (current.Id == toRelase.Id)
                return toRelase.TransactionWaitingLock;

            current.TransactionWaitingLock = ReleaseRecursively(toRelase, current.TransactionWaitingLock);
            return current;


        }
    }
}
