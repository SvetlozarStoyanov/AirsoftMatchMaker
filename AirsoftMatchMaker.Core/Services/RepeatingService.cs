using AirsoftMatchMaker.Core.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AirsoftMatchMaker.Core.Services
{
    public class RepeatingService : BackgroundService
    {
        private readonly PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromSeconds(20));
        //private readonly IGameSimulationService gameSimulationService;
        public RepeatingService(IServiceProvider services/*, IGameSimulationService gameSimulationService*/)
        {
            //this.gameSimulationService = gameSimulationService;
            Services = services;
        }
        public IServiceProvider Services { get; }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Consume Scoped Service Hosted Service is working");
            //while (await timer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested)
            //{
            //var gameIds = await gameSimulationService.FindGamesToSimulateAsync(DateTime.Now);
            //foreach (var gameId in gameIds)
            //{
            //    await gameSimulationService.SimulateGameAsync(gameId);
            //    Console.WriteLine($"Game with id {gameId} has been simulated!");
            //}
            //}
            await DoWork(stoppingToken);
        }

        private async Task DoWork(CancellationToken stoppingToken)
        {

            using (var scope = Services.CreateScope())
            {
                var scopedProcessingService =
                    scope.ServiceProvider
                        .GetRequiredService<IScopedProcessingService>();

                await scopedProcessingService.DoWork(stoppingToken);
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Consume Scoped Service Hosted Service is stopping.");

            await base.StopAsync(stoppingToken);
        }
    }
}
