using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Services;
using AirsoftMatchMaker.Infrastructure.Data.Common.Repository;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AirsoftMatchmakerServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IRepository, Repository>();
            services.AddScoped<IPlayerService, PlayerService>();
            services.AddScoped<IWeaponService, WeaponService>();
            services.AddScoped<IClothingService, ClothingService>();
            services.AddScoped<IAmmoBoxService, AmmoBoxService>();
            services.AddScoped<IMapService, MapService>();
            services.AddScoped<IGameService, GameService>();
            services.AddScoped<IBetService, BetService>();
            services.AddScoped<IGameModeService, GameModeService>();
            services.AddScoped<ITeamService, TeamService>();
            services.AddScoped<IVendorService, VendorService>();
            services.AddScoped<IMatchmakerService, MatchmakerService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IRoleRequestService, RoleRequestService>();
            services.AddScoped<ITeamRequestService, TeamRequestService>();
            services.AddScoped<IGameSimulationService, GameSimulationService>();
            //services.AddScoped<IScopedProcessingService, ScopedProcessingService>();
            //services.AddHostedService<RepeatingService>();
            return services;
        }
    }
}
