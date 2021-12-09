using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AspConsole.Models;

namespace AspConsole.Servises
{
   public class TaskSchedulerSevice : IHostedService, IDisposable
    {
        private Timer timer;
        private readonly IServiceProvider service;
        private readonly Settings settings;
        private readonly ILogger logger;
        private readonly object syncRoot = new object();
        public TaskSchedulerSevice(IServiceProvider service)
        {
            this.service = service;
            this.settings = service.GetRequiredService<Settings>();
            this.logger = service.GetRequiredService<ILogger<TaskSchedulerSevice>>();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        { 
            var interval = settings?.RunInterval ?? 0;
            if (interval == 0)
            {
                logger.LogWarning("Проверь интервал, он не настроен. Настройка интервала = 60 сек.");
                interval = 60;
            }
            timer = new Timer((e) => ProcessTask(), null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
            return Task.CompletedTask;
        }

        private void ProcessTask()
        {
            if (Monitor.TryEnter(syncRoot))
            {
                logger.LogInformation("процесс начался");
                logger.LogInformation("процесс завершился");
                Monitor.Exit(syncRoot);
            }else logger.LogInformation("процесс в стадии выполнения....");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}
