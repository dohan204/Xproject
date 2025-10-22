using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using TestX.application.Repositories;
using TestX.domain.Entities.AccountRole;
using TestX.infrastructure.Identity;
using TestX.application.Dtos.Role;

namespace TestX.infrastructure.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IdentityContext _context;
        public RoleService(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager,
            IdentityContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }
        public async Task<ApplicationRole?> GetRoleById(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if(role == null)
                return null;
            return role;
        }
        public async Task<IdentityResult> CreateRole(CreateRoleDto roleDto)
        {
            // kiểm tra xem role đã tồn tại hay chưa
            var role = await _roleManager.RoleExistsAsync(roleDto.Name);
            // nếu đã tồn tại thì hiển thị ra lỗi kh thể tạo thêm role có cùng tên.
            if(role)
                return IdentityResult.Failed(new IdentityError { Description = "Role đã tồn tại." });

            var newRole = new ApplicationRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = roleDto.Name,
                Description = roleDto.Description,
                CreatedAt = DateTime.UtcNow,
                Active = true,
            };

            IdentityResult result = await _roleManager.CreateAsync(newRole);
            if(!result.Succeeded)
                return IdentityResult.Failed(new IdentityError { Description = "Tạo role không thành công." });

            // lấy function hiện có 
            var allFunction = await _context.Functions.ToListAsync();
            // tạo các permission mặc định cho role mới tạo
            foreach (var function in allFunction)
            {
                var rolePermission = new RolePermission
                {
                    RoleId = newRole.Id,
                    FunctionId = function.Id,
                    CanCreate = false,
                    CanRead = false,
                    CanUpdate = false,
                    CanDelete = false
                };
                _context.RolePermissions.Add(rolePermission);
            }
            await _context.SaveChangesAsync();
            return IdentityResult.Success;

        }
        public async Task<List<ApplicationRole>> GetAllRole()
        {
            return await _roleManager.Roles.ToListAsync();
            //List<IdentityResult> result = new List<IdentityResult>();
            //foreach (var role in roles)
            //{
            //    result.Add(IdentityResult.Success);
            //}
            //return result;
        }
        public async Task<IdentityResult> UpdateRole(string roleId,UpdateRoleDto roleDto)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if(role == null)
                return IdentityResult.Failed(new IdentityError { Description = "Role không tồn tại." });

            var nameRoleExists = await _roleManager.RoleExistsAsync(roleDto.Name);
            if(nameRoleExists && role.Name != roleDto.Name)
                return IdentityResult.Failed(new IdentityError { Description = "Role đã tồn tại." });
            role.Name = roleDto.Name;
            role.Description = roleDto.Description;
            role.Active = roleDto.Active;
            role.UpdatedAt = DateTime.UtcNow;
            foreach(var permission in roleDto.RolePermissionDtos)
            {
                var rolePermission = await _context.RolePermissions
                    .FirstOrDefaultAsync(rp => rp.RoleId == role.Id && rp.FunctionId == permission.FunctionId);
                //if (rolePermission == null)
                //{
                //    rolePermission = new RolePermission
                //    {
                //        RoleId = role.Id,
                //        FunctionId = permission.FunctionId,
                //        // gán quyền từ DTO
                //    };
                //    _context.RolePermissions.Add(rolePermission);
                //}
                if (rolePermission != null)
                {
                    rolePermission.CanCreate = permission.CanCreate;
                    rolePermission.CanRead = permission.CanRead;
                    rolePermission.CanUpdate = permission.CanUpdate;
                    rolePermission.CanDelete = permission.CanDelete;
                    rolePermission.CanModify = permission.CanModify;
                }
            }
            await _context.SaveChangesAsync();
            IdentityResult result = await _roleManager.UpdateAsync(role);
            if(!result.Succeeded)
                return IdentityResult.Failed(new IdentityError { Description = "Cập nhật role không thành công." });
            return result;
        }
        public async Task<IdentityResult> DeleteRole(string name)
        {
            // Implementation for deleting a role
            var role = await _roleManager.FindByNameAsync(name);
            if(role == null)
                return IdentityResult.Failed(new IdentityError { Description = "Role không tồn tại." });
            IdentityResult result = await _roleManager.DeleteAsync(role);
            if(!result.Succeeded)
                return IdentityResult.Failed(new IdentityError { Description = "Xóa role không thành công." });
            return IdentityResult.Success;
        }
        public async Task<IdentityResult> AssignRoleToUser(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "người dung không tồn tại." });
            var role = await _roleManager.RoleExistsAsync(roleName);
            if (!role)
                return IdentityResult.Failed(new IdentityError { Description = "vai trò kh tồn tại, kh thể gán" });
            IdentityResult result = await _userManager.AddToRoleAsync(user, roleName);
            return result;
        }
        public async Task<IdentityResult> CheckExistsRole(string name)
        {
            var role = await _roleManager.FindByNameAsync(name);
            if(role == null) return IdentityResult.Failed(new IdentityError { Description = "Role không tồn tại." });
            var result = await _roleManager.RoleExistsAsync(role.Name!);
            return result ? IdentityResult.Success : IdentityResult.Failed(new IdentityError { Description = "Role không tồn tại." });
        }

        public async Task<IdentityResult> IsUserInRole(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "người dung không tồn tại." });
            var isInRole = await _userManager.IsInRoleAsync(user, roleName);
            return isInRole ? IdentityResult.Success : IdentityResult.Failed(new IdentityError { Description = "Người dùng không có vai trò này." });
        }
        public async Task<IdentityResult> UpdateRoleToUser()
        {
            // Implementation for updating role to user

            return IdentityResult.Success;
        }
    }
}
