using AirsoftMatchMaker.Core.Models.Users;
using AirsoftMatchMaker.Infrastructure.Data.Entities;

namespace AirsoftMatchMaker.Core.Contracts
{
    public interface IUserService
    {
        /// <summary>
        /// Returns <see cref="User.Credits"/>
        /// </summary>
        /// <param name="userId"></param>
        /// <returns><see cref="decimal"/></returns>
        Task<decimal> GetUserCreditsAsync(string userId);

        /// <summary>
        /// Returns <see cref="UserViewModel"/> with information about <see cref="User"/>
        /// </summary>
        /// <param name="userId"></param>
        /// <returns><see cref="UserViewModel"/></returns>
        Task<UserViewModel> GetCurrentUserProfileAsync(string userId);
    }
}
