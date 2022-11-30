using AirsoftMatchMaker.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirsoftMatchMaker.Core.Services
{
    public class ScopedProcessingService : IScopedProcessingService
    {
        private readonly PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromSeconds(10));
        private readonly IGameSimulationService gameSimulationService;
        public ScopedProcessingService(IGameSimulationService gameSimulationService)
        {
            this.gameSimulationService = gameSimulationService;
        }

        public async Task DoWork(CancellationToken stoppingToken)
        {
            while (await timer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested)
            {
                var gameIds = await gameSimulationService.FindGamesToSimulateAsync(DateTime.Now);
                foreach (var gameId in gameIds)
                {
                    await gameSimulationService.SimulateGameAsync(gameId);
                    Console.WriteLine($"Game with id {gameId} has been simulated!");
                }
            }
        }
    }
}
