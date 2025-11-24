using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using TestX.application.Dtos.Role;
using TestX.application.Repositories;
using TestX.domain.Entities.AccountRole;
using TestX.infrastructure.Identity;

namespace TestX.infrastructure.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IdentityContext _context;
        private readonly IMapper _mapper; 
        public RoleService(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager, 
            IdentityContext context, IMapper mapper)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
            _mapper = mapper;
        }
        public async Task<int> GetCountRoleAsync()
        {
            return await _context.Roles.CountAsync();
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
            // lấy ra id của vai trò.
            var role = await _roleManager.FindByIdAsync(roleId);
            // kiểm tra xem vai trò có tồn tại haykhoong
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
        public async Task<IdentityResult> DeleteRoleByName(string name)
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
        public async Task<IdentityResult> AddRoleForUser(AddRoleToUser updateRoleForUser)
        {
            var user =await _userManager.FindByIdAsync(updateRoleForUser.userID);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "Không tìm thấy người dùng." });
            var existRole = await _userManager.IsInRoleAsync(user, updateRoleForUser.roleName);
            if (existRole)
                return IdentityResult.Failed(new IdentityError { Description = "Không thể thêm lại vai trò này vào người dùng" });
            var updateRoleUser = await _userManager.AddToRoleAsync(user, updateRoleForUser.roleName);
            if (!updateRoleUser.Succeeded)
                return IdentityResult.Failed(new IdentityError { Description = "Không cập nhật vai trò của người dùn thành công." });
            return IdentityResult.Success;
        }
        public async Task<IdentityResult> UpdateRoleForUser(UpdateRoleForUser updateRoleForUser)
        {
            // lấy ra thông tin người dùng,
            var user = await _userManager.FindByIdAsync(updateRoleForUser.userId);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "Không tìm thấy người dùng." });
            var currentRoleUser = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            // nếu đã tìm thấy thì thực hienj xóa người dùng khỏi vai trò trước đó để thcujw hiện cập nhật vai trò mới 
            var removeRole = await _userManager.RemoveFromRoleAsync(user, currentRoleUser!);
            // thêm role mới 
            var addRole = await _userManager.AddToRoleAsync(user, updateRoleForUser.roleName);
            if (!addRole.Succeeded)
                return IdentityResult.Failed(new IdentityError { Description = "Cập nhật không thành công khi thêm vai trò mới." });
            return addRole.Succeeded ? IdentityResult.Success : IdentityResult.Failed(new IdentityError { Description = "Cập nhật không thành công." });
        }
        public async Task<List<UserRoleViewModel>> GetUserByRole()
        {
            var users = _userManager.Users.ToList();

            var result = new List<UserRoleViewModel>();
            foreach(var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                result.Add(new UserRoleViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    FullName = user.FullName,
                    Email = user.Email,
                    IsActive = user.Active,
                    RoleName = roles.ToList()

                });

            }
            return result;
        }
        public async Task<IdentityResult> DeleteRole(string id)
        {
            // check role exists
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                return IdentityResult.Failed(new IdentityError { Description = "Role không tồn tại." });

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // remove role from all users who have it
                var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name!);
                if (usersInRole.Any())
                {
                    foreach (var user in usersInRole)
                    {
                        var removeResult = await _userManager.RemoveFromRoleAsync(user, role.Name!);
                        if (!removeResult.Succeeded)
                        {
                            await transaction.RollbackAsync();
                            return IdentityResult.Failed(removeResult.Errors.ToArray());
                        }
                    }
                }

                // remove related role permissions (if any)
                var permissions = await _context.RolePermissions
                    .Where(rp => rp.RoleId == role.Id)
                    .ToListAsync();
                if (permissions.Any())
                {
                    _context.RolePermissions.RemoveRange(permissions);
                    await _context.SaveChangesAsync();
                }

                // delete role itself
                var deleteResult = await _roleManager.DeleteAsync(role);
                if (!deleteResult.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return IdentityResult.Failed(deleteResult.Errors.ToArray());
                }

                await transaction.CommitAsync();
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return IdentityResult.Failed(new IdentityError { Description = $"Lỗi khi xóa vai trò: {ex.Message}" });
            }
        }

        public async Task<IdentityResult> UpdatePermissionForRole(string roleId, List<UpdatePermisstion> dtos)
        {
            // 1. Validate Role tồn tại
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
                return IdentityResult.Failed(new IdentityError { Description = "Vai trò không tồn tại" });

            // Danh sách lỗi để tổng hợp (nếu cần)
            var errors = new List<IdentityError>();

            foreach (var dto in dtos)
            {
                // 2. Validate Function tồn tại
                var function = await _context.Functions.FindAsync(dto.FunctionId);
                if (function == null)
                {
                    errors.Add(new IdentityError { Description = $"Function không tồn tại: ID = {dto.FunctionId}" });
                    continue; // Bỏ qua function này, tiếp tục với cái khác
                }

                // 3. Tìm RolePermission hiện tại
                var existing = await _context.RolePermissions
                    .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.FunctionId == dto.FunctionId);

                if (existing == null)
                {
                    // Tạo mới
                    var newPermission = new RolePermission
                    {
                        RoleId = roleId,
                        FunctionId = dto.FunctionId,
                        Name = dto.Name,
                        CanCreate = dto.CanCreate,
                        CanRead = dto.CanRead,
                        CanUpdate = dto.CanWrite,
                        CanDelete = dto.CanDelete
                    };
                    _context.RolePermissions.Add(newPermission);
                }
                else
                {
                    // Cập nhật
                    existing.Name = dto.Name;
                    existing.CanCreate = dto.CanCreate;
                    existing.CanRead = dto.CanRead;
                    existing.CanUpdate = dto.CanWrite;
                    existing.CanDelete = dto.CanDelete;
                }
            }

            // Nếu có lỗi validate function
            if (errors.Any())
            {
                return IdentityResult.Failed(errors.ToArray());
            }

            // 4. SaveChanges
            try
            {
                await _context.SaveChangesAsync();
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"Lỗi khi lưu dữ liệu: {ex.Message}" });
            }
        }
        public async Task<List<UpdatePermisstion>> GetPermissionsByRole(string roleId)
        {
            var permissions = await _context.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .Select(rp => new UpdatePermisstion
                {
                    FunctionId = rp.FunctionId,
                    Name = rp.Name,
                    CanCreate = rp.CanCreate,
                    CanRead = rp.CanRead,
                    CanWrite = rp.CanUpdate,
                    CanDelete = rp.CanDelete
                })
                .ToListAsync();
            return permissions;
        }
    }
}
