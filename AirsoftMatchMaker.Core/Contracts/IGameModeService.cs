using AirsoftMatchMaker.Core.Models.GameModes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirsoftMatchMaker.Core.Contracts
{
    public interface IGameModeService
    {
        /// <summary>
        /// Returns all game modes
        /// </summary>
        /// <returns><see cref="IEnumerable{T}"/></returns>
        Task<IEnumerable<GameModeListModel>> GetAllGameModesAsync();

        /// <summary>
        /// Returns game mode by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="GameModeViewModel"/></returns>
        Task<GameModeViewModel> GetGameModeByIdAsync(int id);
    }
}
