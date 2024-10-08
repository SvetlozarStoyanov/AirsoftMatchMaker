﻿using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Services;
using AirsoftMatchMaker.Infrastructure.Data.DataAccess.UnitOfWork;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AirsoftMatchmakerServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IPlayerService, PlayerService>();
            services.AddScoped<IPlayerClassService, PlayerClassService>();
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
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IRoleRequestService, RoleRequestService>();
            services.AddScoped<ITeamRequestService, TeamRequestService>();
            services.AddScoped<IInventoryService, InventoryService>();
            services.AddScoped<IGameSimulationService, GameSimulationService>();
            services.AddScoped<IBackgroundTeamService, BackgroundTeamService>();
            services.AddScoped<IBackgroundGameService, BackgroundGameService>();
            services.AddScoped<IScopedProcessingService, ScopedProcessingService>();
            services.AddScoped<IHtmlSanitizingService, HtmlSanitizingService>();

            services.AddHostedService<RepeatingService>();
            return services;
        }
    }
}
