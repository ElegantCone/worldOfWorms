using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WormsWorld.AI;
using WormsWorld.Engine;
using WormsWorld.Model;
using WormsWorld.Services;

namespace WormsWorld
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<SimulatorHostedService>();
                    services.AddSingleton<Field>();
                    services.AddSingleton<FoodGenerator>();
                    services.AddSingleton<Logger>();
                    services.AddSingleton<NameGenerator>();
                    services.AddSingleton<WormBehaviourService>();
                    services.AddSingleton<Simulator>();
                });
        }

    }
}