using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using TestX.application.Dtos.Role;
using TestX.application.Repositories;

namespace TestX.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly ILogger<RoleController> _logger;
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService, ILogger<RoleController> logger)
        {
            _roleService = roleService;
            _logger = logger;
        }
        [HttpGet("get-AllRole")]
        public async Task<IActionResult> GetAllRoleAsync()
        {
            try
            {
                var roles = await _roleService.GetAllRole();
                return Ok(roles);
            }
            catch (Exception ex)
            {
                _logger.LogError("có lỗi sảy ra khi lấy dữ liệu {error}", ex.InnerException);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("get-RoleById/{id}")]
        public async Task<IActionResult> GetRoleByIdAsync(string id)
        {
            try
            {
                var role = await _roleService.GetRoleById(id);
                if (role == null)
                    return NotFound();
                return Ok(role);
            }
            catch (Exception ex)
            {
                _logger.LogError("có lỗi sảy ra khi lấy dữ liệu theo id {error}", ex.InnerException);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("getByName")]
        public async Task<IActionResult> ExistsRole([FromQuery] string name)
        {
            try
            {
                var role = await _roleService.CheckExistsRole(name);
                if (role == null)
                    return Content("Vai trò bạn cần tìm không tồn tại");
                return Ok(role);

            }
            catch (Exception ex)
            {
                _logger.LogError("Sảy ra lỗi khi thực hiện thao tac get: {error}", ex.InnerException);
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost("create-Role")]
        public async Task<IActionResult> CreateRoleAsync([FromBody] CreateRoleDto roleDto)
        {
            try
            {
                var result = await _roleService.CreateRole(roleDto);
                if (!result.Succeeded)
                    return BadRequest(result.Errors);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("có lỗi sảy ra khi tạo role {error}", ex.InnerException);
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("update-Role")]
        public async Task<IActionResult> Update([FromBody] UpdateRoleDto role, string roleId)
        {
            try
            {
                var update = await _roleService.UpdateRole(roleId, role);
                if (!update.Succeeded)
                    return BadRequest();
                return Content("Cập nhật vai trò thành công.");

            }
            catch (DbUpdateException ex)
            {
                _logger.LogError("Sảy ra lỗi khi thực hiện cập nhật: {error}", ex.InnerException);
                return StatusCode(500, ex.Message);
            }
            catch (DbException ex)
            {
                _logger.LogError("lỗi gì ấy: {error}", ex.InnerException);
                return StatusCode(500, ex.Message);
            }
        }
        [HttpDelete("delete_role")]
        public async Task<IActionResult> DeleteRole(string roleName)
        {
            try
            {
                var role =await _roleService.DeleteRole(roleName);
                return Content("Xóa vai trò thành công.");
            }
            catch (Exception ex)
            {
                _logger.LogError("Không thực hiện được yeu cầu xóa. {error}", ex.InnerException);
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost("assignRole")]
        public async Task<IActionResult> AssignRole(string userID, string roleName)
        {
            try
            {
                var userRole = await _roleService.AssignRoleToUser(userID, roleName);
                return Content("Thiết lập vai chò cho {userID} thành công.", userID);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError("Xảy ra lỗi thi thực hiện thao tác này.{error}", ex.InnerException);
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("CheckRoleUser")]
        public async Task<IActionResult> Check(string userID, string roleName)
        {
            try
            {
                var user = await _roleService.IsUserInRole(userID, roleName);
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError("Không có kết quả: {error}", ex.InnerException);
                    return StatusCode(500, ex.Message);
                //throw;
            }
        }
    }
}
