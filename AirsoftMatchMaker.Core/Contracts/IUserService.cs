using AirsoftMatchMaker.Core.Models.Users;

namespace AirsoftMatchMaker.Core.Contracts
{
    public interface IUserService
    {
        Task<UserViewModel> GetCurrentUserProfileAsync(string userId);
    }
}
