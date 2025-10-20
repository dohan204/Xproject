using Microsoft.AspNetCore.Mvc;
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
    }
}
