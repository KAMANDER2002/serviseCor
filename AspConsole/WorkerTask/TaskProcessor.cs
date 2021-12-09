using AspConsole.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AspConsole.WorkerTask
{
   public class TaskProcessor
    {
        private readonly ILogger<TaskProcessor> ilogger;
        private readonly ILogger<TaskProcessor> logger;
        private readonly Settings settings;

        public TaskProcessor(ILogger<TaskProcessor> logger, Settings settings)
        {
            this.logger = logger;
            this.settings = settings;
        }
        // Расчет ряда фибоначи
        public async Task RunAsunk(int number, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            Func<int, int> fibonacci = null;
            fibonacci = (num) =>
            {
                if (num < 2) return 1;
                else return fibonacci(num - 1) + fibonacci(num - 2);
            };
            var result = await Task.Run(async () =>
            {
                await Task.Delay(1000);
                return Enumerable.Range(0, number).Select(n => fibonacci(n));//Берем вводим number и по числу number выводим числа фибоначи
            },token);
            using (var writer = new StreamWriter(settings.ResultPath, true, Encoding.UTF8))
            {
                writer.WriteLine(DateTime.Now.ToString() + ":" + string.Join(" ", result));
            }
            logger.LogInformation($"Процесс завершен. Результат {string.Join(" ",result)}");
        }
    }
}
