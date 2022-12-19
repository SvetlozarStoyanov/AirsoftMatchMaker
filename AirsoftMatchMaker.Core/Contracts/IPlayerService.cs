using AirsoftMatchMaker.Core.Models.PlayerClasses;
using AirsoftMatchMaker.Core.Models.Players;
using AirsoftMatchMaker.Infrastructure.Data.Entities;

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

        Task<decimal> GetPlayersAvailableCreditsAsync(string userId);

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
