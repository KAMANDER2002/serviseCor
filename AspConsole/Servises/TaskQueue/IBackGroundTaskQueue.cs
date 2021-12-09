

using System;
using System.Threading;
using System.Threading.Tasks;

namespace AspConsole.Servises.TaskQueue
{
    public interface IBackGroundTaskQueue
    {
        int Size { get; }
        void QueueBackGroundWorkItem(Func<CancellationToken, Task> workItem);
        Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellation);
    }
}
