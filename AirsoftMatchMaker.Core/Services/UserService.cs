using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.Users;
using AirsoftMatchMaker.Infrastructure.Data.DataAccess.BaseRepository;
using AirsoftMatchMaker.Infrastructure.Data.DataAccess.UnitOfWork;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AirsoftMatchMaker.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<decimal> GetUserCreditsAsync(string userId)
        {
            var user = await unitOfWork.UserRepository.GetByIdAsync(userId);

            return user.Credits;
        }

        public async Task<UserViewModel> GetCurrentUserProfileAsync(string userId)
        {
            var user = await unitOfWork.UserRepository.AllReadOnly()
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
