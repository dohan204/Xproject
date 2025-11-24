using Microsoft.AspNetCore.Mvc;
using TestX.application.Dtos.Role;
using TestX.domain.Entities.AccountRole;
using TestX.application.Repositories;
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

    [HttpGet]
    public async Task<IActionResult> GetAllRoles()
    {
        var roles = await _roleService.GetAllRole();
        return Ok(roles);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRoleById(string id)
    {
        var role = await _roleService.GetRoleById(id);
        return role is null ? NotFound() : Ok(role);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto dto)
    {
        var result = await _roleService.CreateRole(dto);
        return result.Succeeded ? Ok(result) : BadRequest(result.Errors);
    }

    [HttpPut("{roleId}")]
    public async Task<IActionResult> UpdateRole(string roleId, [FromBody] UpdateRoleDto dto)
    {
        var result = await _roleService.UpdateRole(roleId, dto);
        return result.Succeeded ? Ok("Cập nhật thành công.") : BadRequest(result.Errors);
    }

    [HttpDelete("{roleName}")]
    public async Task<IActionResult> DeleteRole(string roleName)
    {
        await _roleService.DeleteRole(roleName);
        return Ok("Xóa vai trò thành công.");
    }

    // ✅ Gán vai trò cho User (Chuẩn – nhận model trong Body)
    [HttpPost("assign")]
    public async Task<IActionResult> AssignRole([FromBody] AddRoleToUser dto)
    {
        var result = await _roleService.AddRoleForUser(dto);
        return result.Succeeded ? Ok("Gán vai trò thành công.") : BadRequest(result.Errors);
    }

    // ✅ Cập nhật role User
    [HttpPut("change")]
    public async Task<IActionResult> ChangeUserRole([FromBody] UpdateRoleForUser dto)
    {
        var result = await _roleService.UpdateRoleForUser(dto);
        return result.Succeeded ? Ok("Cập nhật vai trò thành công.") : BadRequest(result.Errors);
    }

    [HttpGet("user-in-role")]
    public async Task<IActionResult> CheckUserInRole([FromQuery] string userId, [FromQuery] string roleName)
    {
        var isInRole = await _roleService.IsUserInRole(userId, roleName);
        return Ok(isInRole);
    }

    [HttpGet("with-users")]
    public async Task<IActionResult> GetUsersWithRoles()
    {
        var data = await _roleService.GetUserByRole();
        return Ok(data);
    }

    [HttpPut]
    public async Task<IActionResult> UpdatePermission(string roleId, [FromBody] List<UpdatePermisstion> dto)
    {
        var result = await _roleService.UpdatePermissionForRole(roleId, dto);
        return result.Succeeded ? Ok("Cập nhật quyền thành công.") : BadRequest(result.Errors);
    }
    [HttpGet("CountRole")]
    public async Task<IActionResult> GetCountRole()
    {
        try
        {
            var role = await _roleService.GetCountRoleAsync();
            return Ok(role);
        } 
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting role count.");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
    [HttpPost("UpdateOrCreate")]
    public async Task<IActionResult> Update(string Id, List<UpdatePermisstion> dto)
    {
        try
        {
            var rolePermission = await _roleService.UpdatePermissionForRole(Id, dto);
            if (!rolePermission.Succeeded)
                return BadRequest();
            return Ok("Thanhf cong.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "looix roi");
            return StatusCode(500);
        }
    }
    [HttpGet("GetPermision")]
    public async Task<IActionResult> GetDetails(string roleId)
    {
        try
        {
            var perMissionDetails = _roleService.GetPermissionsByRole(roleId);
            if (perMissionDetails == null)
                return NotFound();
            return Ok(perMissionDetails);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi rồi.");
            return StatusCode(500);
        }
    }
}
