using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestX.application.Repositories
{
    public interface IRoleService
    {
        Task<List<IdentityResult>> GetAllRole();
        Task<IdentityResult> GetRoleById(string id);
        Task<IdentityResult> CreateRole(string roleName);
        Task<IdentityResult> UpdateRole();
        Task<IdentityResult> DeleteRole(string roleName);
        Task<IdentityResult> AssignRoleToUser(string userId, string roleName);
        Task<IdentityResult> UpdateRoleToUser();
        Task<IdentityResult> CheckExistsRole(string name);
        Task<IdentityResult> IsUserInRole(string userId, string roleName);
    }
}
