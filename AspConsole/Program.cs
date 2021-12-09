using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using AspConsole.Servises;

namespace AspConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new HostBuilder().ConfigureAppConfiguration(configBuilder=>
            {
                configBuilder.AddJsonFile("config.json");
                configBuilder.AddCommandLine(args);
            })
            .ConfigureLogging((ConfigLogger)=> 
            {
                ConfigLogger.AddConsole();
                ConfigLogger.AddDebug();
            })
            .ConfigureServices((servises)=> 
            {
                servises.AddHostedService<TaskSchedulerSevice>();
                servises.AddHostedService<WorkerServeces>();
                servises.AddSingleton<Models.Settings>();
                servises.AddSingleton<WorkerTask.TaskProcessor>();
                servises.AddSingleton<Servises.TaskQueue.IBackGroundTaskQueue, Servises.TaskQueue.BackgroundTaskQueu>();
            });

            await builder.RunConsoleAsync();
        }
    }
}
