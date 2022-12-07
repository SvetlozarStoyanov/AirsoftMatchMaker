using AirsoftMatchMaker.Core.Contracts;


namespace AirsoftMatchMaker.Core.Services
{
    public class ScopedProcessingService : IScopedProcessingService
    {
        private PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromSeconds(10));
        private readonly IGameSimulationService gameSimulationService;
        private readonly IBackgroundTeamService backgroundTeamService;
        public ScopedProcessingService(IGameSimulationService gameSimulationService, IBackgroundTeamService backgroundTeamService)
        {
            this.gameSimulationService = gameSimulationService;
            this.backgroundTeamService = backgroundTeamService;
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
                var teamRequestIds = await backgroundTeamService.GetPlayersToAssignOrRemoveFromTeamsTeamRequestIdsAsync();
                if (teamRequestIds.Any())
                {
                    await backgroundTeamService.AssignOrRemovePlayerFromTeamByTeamRequestIdsAsync(teamRequestIds);
                }
                timer = new PeriodicTimer(TimeSpan.FromMinutes(1));
            } while (await timer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested);

        }
    }
}
