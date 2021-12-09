using AspConsole.Models;
using AspConsole.Servises.TaskQueue;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AspConsole.Servises
{
    public class WorkerServeces : BackgroundService
    {
        private readonly IBackGroundTaskQueue taskQueue;
        private readonly ILogger<WorkerServeces> iloger;
        private readonly Settings settings;

        public WorkerServeces(Servises.TaskQueue.IBackGroundTaskQueue taskQueue, ILogger<WorkerServeces> iloger, Models.Settings settings)
        {
            this.taskQueue = taskQueue;
            this.iloger = iloger;
            this.settings = settings;
        }
        protected override async Task ExecuteAsync(CancellationToken token)
        {
            var workersCount = settings.WorkersCount;
           var workers = Enumerable.Range(0, workersCount).Select(num => RunInstance(num, token));
           await Task.WhenAll(workers);
        }

        private async Task RunInstance(int num, CancellationToken token)
        {
              iloger.LogInformation($"#{num} is starting");
            while (!token.IsCancellationRequested)
            {
               await Task.Delay(1000);
               var workItem = await taskQueue.DequeueAsync(token);
                try
                {
                    iloger.LogInformation($"#{num}:processing task. Queue size: {taskQueue.Size}.");
                    await workItem(token);
                }
                catch(Exception ex)
                {
                    iloger.LogInformation(ex, $"#{num}: Error");
                }
            }
            iloger.LogInformation($"#{num} is finished");

        }
    }
}
