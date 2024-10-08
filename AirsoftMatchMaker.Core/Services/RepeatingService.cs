using AirsoftMatchMaker.Core.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AirsoftMatchMaker.Core.Services
{
    public class RepeatingService : BackgroundService
    {
        public RepeatingService(IServiceProvider services)
        {
            Services = services;
        }
        public IServiceProvider Services { get; }
        protected override async Task ExecuteAsync  (CancellationToken stoppingToken)
        {
            Console.WriteLine("Consume Scoped Service Hosted Service is working");
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
