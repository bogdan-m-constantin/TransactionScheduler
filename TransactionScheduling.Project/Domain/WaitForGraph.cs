﻿namespace TransactionScheduling.Project.Domain
{
    public class WaitForGraph(ILogger<WaitForGraph> logger)
    {
        public ILogger<WaitForGraph> _logger = logger;
        private readonly List<Table> _tables = [];

        public void Init()
        {
            _tables.Clear();
            _tables.Add(new Table("Clients"));
            _tables.Add(new Table("Orders"));
            _tables.Add(new Table("OrderItems"));
            _tables.Add(new Table("Products"));
            _tables.Add(new Table("ProductPriceChanges"));
            _tables.Add(new Table("ProductStockChanges"));

        }

        public WaitForGraphNode? GetDeadLock()
        {
            foreach (var table in _tables)
            {
                var visited = new List<Guid>();
                var t = table.TransactionHasLock?.TransactionWaitingLock;

                while (t != null)
                {
                    if (visited.Contains(t.Id))
                    {
                        return t;
                    }
                    visited.Add(t.Id);
                    t = t.TransactionWaitingLock;
                }
            }
            return null;
        }


        internal WaitForGraphNode AquireLock(string tableName, Transaction transaction)
        {
            var table = _tables.Find(e => e.Name == tableName);
            return table == null ? throw new Exception($"Could not aqurie lock on table {tableName}") : table.AquireLock(transaction);
        }

        internal static void Release(WaitForGraphNode node)
        {
            node.LockTable!.Release(node);
        }
    }
}
