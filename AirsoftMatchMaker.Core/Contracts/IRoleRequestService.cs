using AirsoftMatchMaker.Core.Models.RoleRequests;

namespace AirsoftMatchMaker.Core.Contracts
{
    public interface IRoleRequestService
    {
        /// <summary>
        /// Returns all role requests
        /// </summary>
        /// <returns><see cref="IEnumerable{RoleRequestListModel}"/></returns>
        Task<IEnumerable<RoleRequestListModel>> GetAllRoleRequestsAsync();

        /// <summary>
        /// Returns role request with given Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="RoleRequestViewModel"/></returns>
        Task<RoleRequestViewModel> GetRoleRequestByIdAsync(int id);


        /// <summary>
        /// Creates a role request which an admin must approve or disapprove
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task CreateRoleRequestAsync(string roleId, string userId);

        /// <summary>
        /// Deletes a role request with given roleId and userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task DeleteRoleRequestAsync(string roleId, string userId);

        /// <summary>
        /// Deletes RoleRequest with given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteRoleRequestAsync(int id);
    }
}
