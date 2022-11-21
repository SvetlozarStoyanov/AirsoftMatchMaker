using AirsoftMatchMaker.Core.Models.Roles;

namespace AirsoftMatchMaker.Core.Contracts
{
    public interface IRoleService
    {
        /// <summary>
        /// Returns roles which the user can request
        /// </summary>
        /// <returns><see cref="IEnumerable{T}"/></returns>
        Task<IEnumerable<RoleListModel>> GetRequestableRolesAsync(IEnumerable<string> roles);

        /// <summary>
        /// Returns roles which the user has
        /// </summary>
        /// <returns><see cref="IEnumerable{T}"/></returns>
        Task<IEnumerable<RoleListModel>> GetUserRolesAsync(IEnumerable<string> roles);

        /// <summary>
        /// Returns roles with given ids
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns><see cref="RoleListModel"/></returns>
        Task<IEnumerable<RoleListModel>> GetRoleByIdsAsync(IEnumerable<string> roleIds);
    }
}
