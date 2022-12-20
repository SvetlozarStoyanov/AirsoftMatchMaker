﻿using AirsoftMatchMaker.Core.Models.PlayerClasses;
using AirsoftMatchMaker.Core.Models.Players;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using AirsoftMatchMaker.Infrastructure.Data.Enums;

namespace AirsoftMatchMaker.Core.Contracts
{
    public interface IPlayerService
    {
        /// <summary>
        /// Returns false if <see cref="Player"/> is in team which has an upcoming <see cref="Game"/>, otherwise returns true.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> CanUserLeavePlayerRole(string userId);

        /// <summary>
        /// Checks for player with given userId and marks them as active, 
        /// if there is no player with this id it creates a new one and marks them as active.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task GrantPlayerRoleAsync(string userId);

        /// <summary>
        /// Checks for player with given userId and marks them as inactive,
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task RemoveFromPlayerRoleAsync(string userId);

        /// <summary>
        /// Returns <see cref="Player"/> <see cref="User.Credits"/> , if player is in a <see cref="Team"/> 
        /// it takes account of <see cref="GameStatus.Upcoming"/> games and subtracts their entry fee from
        /// credits
        /// </summary>
        /// <param name="userId"></param>
        /// <returns><see cref="decimal"/></returns>
        Task<decimal> GetPlayersAvailableCreditsAsync(string userId);

        /// <summary>
        /// Returns <see cref="Player.TeamId"/> if player is in team, otherwise returns null
        /// </summary>
        /// <param name="userId"></param>
        /// <returns><see cref="int"/> or null</returns>
        Task<int?> GetPlayersTeamIdAsync(string userId);


        /// <summary>
        /// Returns all players
        /// </summary>
        /// <returns><see cref="IEnumerable{typeof(PlayerListModel)}"/></returns>
        Task<IEnumerable<PlayerListModel>> GetAllPlayersAsync();

        /// <summary>
        /// Returns player with given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="PlayerViewModel"/></returns>
        Task<PlayerViewModel> GetPlayerByIdAsync(int id);


    }
}
