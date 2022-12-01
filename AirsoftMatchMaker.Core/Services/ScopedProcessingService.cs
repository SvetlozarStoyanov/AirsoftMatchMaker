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
        private PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromSeconds(10));
        private readonly IGameSimulationService gameSimulationService;
        public ScopedProcessingService(IGameSimulationService gameSimulationService)
        {
            this.gameSimulationService = gameSimulationService;
        }

        public async Task DoWork(CancellationToken stoppingToken)
        {
            do
            {
                var gameIds = await gameSimulationService.FindGamesToSimulateAsync(DateTime.Now);
                foreach (var gameId in gameIds)
                {
                    await gameSimulationService.SimulateGameAsync(gameId);
                    await gameSimulationService.PayoutBetsByGameIdAsync(gameId);
                    Console.WriteLine($"Game with id {gameId} has been simulated and it's bets have been paid out!");
                }
                timer = new PeriodicTimer(TimeSpan.FromMinutes(15));
            } while (await timer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested);

        }
    }
}
