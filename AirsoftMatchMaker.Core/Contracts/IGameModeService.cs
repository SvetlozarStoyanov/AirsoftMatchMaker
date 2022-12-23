using AirsoftMatchMaker.Core.Models.Enums;
using AirsoftMatchMaker.Core.Models.GameModes;
using AirsoftMatchMaker.Infrastructure.Data.Entities;


namespace AirsoftMatchMaker.Core.Contracts
{
    public interface IGameModeService
    {
        /// <summary>
        /// Returns true if <see cref="GameMode"/> with given <paramref name="id"/> exists, false otherwise
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="bool"/></returns>
        Task<bool> GameModeExistsAsync(int id);

        /// <summary>
        /// Returns true if <see cref="GameMode"/> with given name already exists, false otherwise
        /// </summary>
        /// <param name="gameModeName"></param>
        /// <returns></returns>
        Task<bool> IsGameModeNameAlreadyTaken(string gameModeName);

        /// <summary>
        /// Returns all game modes
        /// </summary>
        /// <returns><see cref="IEnumerable{T}"/></returns>
        Task<GameModesQueryModel> GetAllGameModesAsync(
            string? searchTerm = null,
            GameModeSorting sorting = GameModeSorting.Newest,
            int gameModesPerPage = 6,
            int currentPage = 1
            );

        /// <summary>
        /// Returns game mode by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="GameModeViewModel"/></returns>
        Task<GameModeViewModel> GetGameModeByIdAsync(int id);

        /// <summary>
        /// Creates a new <see cref="GameMode"/> from <see cref="GameModeCreateModel"/>
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task CreateGameModeAsync(GameModeCreateModel model);
    }
}
