using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using WormsWorld.AI;
using WormsWorld.Engine;
using WormsWorld.Model;

namespace WormsWorld
{
    class SimulatorHostedService : IHostedService
    {
        private IHostApplicationLifetime _appLifetime;
        private Simulator _simulator;

        public SimulatorHostedService(Simulator simulator, IHostApplicationLifetime lifetime)
        {
            _simulator = simulator;
            _appLifetime = lifetime;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(() =>
            {
                Thread.Sleep(250);
                try
                {
                    _simulator.StartGame();
                }
                catch (Exception e)
                {
                    Console.Error.Write(e.ToString());
                }
                finally
                {
                    _appLifetime.StopApplication();
                }
            }, cancellationToken);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}