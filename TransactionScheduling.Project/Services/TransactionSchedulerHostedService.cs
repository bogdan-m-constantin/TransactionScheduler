using TransactionScheduling.Project.Domain;

namespace TransactionScheduling.Project.Services
{
    public class TransactionSchedulerHostedService(WaitForGraph graph) : IHostedService
    {
        private readonly WaitForGraph _graph = graph;
        Thread? t = null;
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _graph.Init();
            t = new Thread(CheckDeadLocksThread);
            t.Start();
            return Task.CompletedTask;
        }

        private void CheckDeadLocksThread(object? obj)
        {
            while (true)
            {
                var node = _graph.GetDeadLock();
                if (node != null)
                    node.Transaction!.CancelationToken!.Cancel();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            t?.Join();
            return Task.CompletedTask;
        }
    }
}
