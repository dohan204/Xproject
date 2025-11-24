using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestX.application.Dtos.Role;
using TestX.domain.Entities.AccountRole;

namespace TestX.application.Repositories
{
    public interface IRoleService
    {
        Task<int> GetCountRoleAsync();
        Task<List<ApplicationRole>> GetAllRole();
        Task<ApplicationRole?> GetRoleById(string id);
        Task<IdentityResult> CreateRole(CreateRoleDto dto);
        Task<IdentityResult> UpdateRole(string roleId, UpdateRoleDto roleDto);
        Task<IdentityResult> DeleteRoleByName(string roleName);
        Task<IdentityResult> AssignRoleToUser(string userId, string roleName);
        Task<IdentityResult> CheckExistsRole(string name);
        Task<IdentityResult> IsUserInRole(string userId, string roleName);
        Task<List<UserRoleViewModel>> GetUserByRole();
        Task<IdentityResult> UpdateRoleForUser(UpdateRoleForUser updateRoleForUser);
        Task<IdentityResult> AddRoleForUser(AddRoleToUser updateRoleForUser);
        Task<IdentityResult> DeleteRole(string id);
        Task<IdentityResult> UpdatePermissionForRole(string role, List<UpdatePermisstion> rolePermission);
        Task<List<UpdatePermisstion>> GetPermissionsByRole(string roleId);
    }
}
