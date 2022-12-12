using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.Users;
using AirsoftMatchMaker.Infrastructure.Data.Common.Repository;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AirsoftMatchMaker.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository repository;

        public UserService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<UserViewModel> GetCurrentUserProfileAsync(string userId)
        {
            var user = await repository.AllReadOnly<User>()
                .Where(u => u.Id == userId)
                .Select(u => new UserViewModel()
                {
                    UserName = u.UserName,
                    Email = u.Email,
                    Credits = u.Credits
                })
                .FirstOrDefaultAsync();
            return user;
        }
    }
}
