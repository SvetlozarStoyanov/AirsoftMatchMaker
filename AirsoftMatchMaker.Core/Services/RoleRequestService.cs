using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.RoleRequests;
using AirsoftMatchMaker.Infrastructure.Data.DataAccess.UnitOfWork;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AirsoftMatchMaker.Core.Services
{
    public class RoleRequestService : IRoleRequestService
    {
        private readonly IUnitOfWork unitOfWork;
        public RoleRequestService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task CreateRoleRequestAsync(string roleId, string userId)
        {
            if (unitOfWork.RoleRequestRepository.All().Any(rrq => rrq.RoleId == roleId && rrq.UserId == userId))
            {
                return;
            }

            var roleRequest = new RoleRequest()
            {
                RoleId = roleId,
                UserId = userId
            };
            await unitOfWork.RoleRequestRepository.AddAsync(roleRequest);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteRoleRequestAsync(string roleId, string userId)
        {
            if (!unitOfWork.RoleRequestRepository.All().Any(rrq => rrq.RoleId == roleId && rrq.UserId == userId))
            {
                return;
            }
            var roleRequest = await unitOfWork.RoleRequestRepository.All().FirstOrDefaultAsync(rrq => rrq.RoleId == roleId && rrq.UserId == userId);
            if (roleRequest == null)
            {
                return;
            }
            unitOfWork.RoleRequestRepository.Delete(roleRequest);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteRoleRequestAsync(int id)
        {
            var roleRequest = await unitOfWork.RoleRequestRepository.GetByIdAsync(id);
            if (roleRequest == null)
            {
                return;
            }
            unitOfWork.RoleRequestRepository.Delete(roleRequest);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<RoleRequestListModel>> GetAllRoleRequestsAsync()
        {
            var roleRequests = await unitOfWork.RoleRequestRepository.AllReadOnly()
                .Include(rrq => rrq.Role)
                .Include(rrq => rrq.User)
                .Select(rrq => new RoleRequestListModel()
                {
                    Id = rrq.Id,
                    RoleId = rrq.RoleId,
                    RoleName = rrq.Role.Name,
                    UserId = rrq.UserId,
                    UserName = rrq.User.UserName
                })
                .ToListAsync();
            return roleRequests;
        }

        public async Task<RoleRequestViewModel> GetRoleRequestByIdAsync(int id)
        {
            var roleRequest = await unitOfWork.RoleRequestRepository.AllReadOnly()
           .Include(rrq => rrq.Role)
           .Include(rrq => rrq.User)
           .Select(rrq => new RoleRequestViewModel()
           {
               Id = rrq.Id,
               RoleId = rrq.RoleId,
               RoleName = rrq.Role.Name,
               UserId = rrq.UserId,
               UserName = rrq.User.UserName
           })
           .FirstOrDefaultAsync();
            return roleRequest;
        }

        public async Task<IEnumerable<RoleRequestListModel>> GetRequestedRolesByUserIdAsync(string userId)
        {
            var requestedRoles = await unitOfWork.RoleRequestRepository.AllReadOnly()
                .Where(rrq => rrq.UserId == userId)
                .Select(rrq => new RoleRequestListModel()
                {
                    Id = rrq.Id,
                    RoleId = rrq.RoleId,
                    RoleName = rrq.Role.Name,
                    UserId = rrq.UserId,
                    UserName = rrq.User.UserName
                })
                .ToListAsync();
            return requestedRoles;
        }
    }
}
