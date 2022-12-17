using AirsoftMatchMaker.Core.Contracts;


namespace AirsoftMatchMaker.Core.Services
{
    public class ScopedProcessingService : IScopedProcessingService
    {
        private PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromSeconds(10));
        //private readonly IGameSimulationService gameSimulationService;
        private readonly IBackgroundTeamService backgroundTeamService;
        private readonly IBackgroundGameService backgroundGameService;
        
        public ScopedProcessingService(IBackgroundGameService backgroundGameService, IBackgroundTeamService backgroundTeamService)
        {
            this.backgroundGameService = backgroundGameService;
            this.backgroundTeamService = backgroundTeamService;
        }

        public async Task DoWork(CancellationToken stoppingToken)
        {
            do
            {
                var gamesToUpdateBettingOddsIds = await backgroundGameService.GetGameIdsOfGamesWithNotUpDoDateOddsAsync();
                foreach (var gameId in gamesToUpdateBettingOddsIds)
                {
                    await backgroundGameService.CalculateBettingOddsAsync(gameId);
                    Console.WriteLine($"Game with id {gameId} odds have been calculated!");
                }
                await backgroundGameService.MarkGamesAsFinishedAsync(DateTime.Now);
                var teamRequestIds = await backgroundTeamService.GetPlayersToAssignOrRemoveFromTeamsTeamRequestIdsAsync();
                if (teamRequestIds.Any())
                {
                    await backgroundTeamService.AssignOrRemovePlayerFromTeamByTeamRequestIdsAsync(teamRequestIds);
                }
                timer = new PeriodicTimer(TimeSpan.FromMinutes(15));
            } while (await timer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested);

        }
    }
}
