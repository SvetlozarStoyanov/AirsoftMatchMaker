using AirsoftMatchMaker.Core.Models.PlayerClasses;
using AirsoftMatchMaker.Infrastructure.Data.Entities;

namespace AirsoftMatchMaker.Core.Contracts
{
    public interface IPlayerClassService
    {
        /// <summary>
        /// Returns true if <see cref="PlayerClass"/> exists, otherwise returns false.
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="bool"/></returns>
        Task<bool> PlayerClassExists(int id);

        /// <summary>
        /// Returns true if <see cref="Player"/> with given <paramref name="userId"/> is already assigned
        /// to  <see cref="PlayerClass"/> with given <paramref name="playerClassId"/>, otherwise returns false.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="playerClassId"></param>
        /// <returns></returns>
        Task<bool> IsPlayerAlreadyInPlayerClass(string userId,int playerClassId);

        /// <summary>
        /// Returns all <see cref="PlayerClass"/> from the database
        /// </summary>
        /// <returns><see cref="IEnumerable{T}"/></returns>
        Task<IEnumerable<PlayerClassListModel>> GetAllPlayerClassesAsync();

        /// <summary>
        /// Returns <see cref="PlayerClass.Id"/> of <see cref="PlayerClass"/> of <see cref="Player"/>
        /// with given <paramref name="userId"/>
        /// </summary>
        /// <param name="userId"></param>
        /// <returns><see cref="int"/></returns>
        Task<int> GetPlayersPlayerClassIdByUserIdAsync(string userId);

        /// <summary>
        /// Assigns <see cref="Player"/> to <see cref="PlayerClass"/> with given <paramref name="playerClassId"/>
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="playerClassId"></param>
        /// <returns></returns>
        Task ChangePlayerClassAsync(string userId, int playerClassId);


        //Task RemovePlayerFromPlayerClassAsync(string userId, int playerClassId);
    }
}
