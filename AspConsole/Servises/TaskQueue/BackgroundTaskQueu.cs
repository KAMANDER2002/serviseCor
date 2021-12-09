using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AspConsole.Servises.TaskQueue
{
    class BackgroundTaskQueu : IBackGroundTaskQueue
    {
        private ConcurrentQueue<Func<CancellationToken, Task>> workItems = new ConcurrentQueue<Func<CancellationToken, Task>>();
        private SemaphoreSlim signal = new SemaphoreSlim(0);
        public int Size { get; }

        public async Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellation)
        {
            await signal.WaitAsync(cancellation);
            workItems.TryDequeue(out var workItem);
            return workItem;
        }

        public void QueueBackGroundWorkItem(Func<CancellationToken, Task> workItem)
        {
            if (workItem == null)
            {
                throw new ArgumentNullException(nameof(workItem));
            }
            workItems.Enqueue(workItem);
            signal.Release();
        }
    }
}
