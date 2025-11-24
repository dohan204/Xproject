using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TestX.api.Dtos.Permission;
using TestX.application.Dtos.AccountAddress;
using TestX.application.Dtos.Role;
using TestX.domain.Entities.AccountRole;
using TestX.infrastructure.Identity;

namespace TestX.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ModuleFunctionController : ControllerBase
    {
        private readonly ILogger<ModuleFunctionController> _logger;
        private readonly IdentityContext _context;
        private readonly IMemoryCache _cache;
        public ModuleFunctionController(ILogger<ModuleFunctionController> logger, IdentityContext context, IMemoryCache cache)
        {
            _logger = logger;
            _context = context;
            _cache = cache;
        }
        [HttpGet("Data")]
        public async Task<IActionResult> GetModuleFunction()
        {
            try
            {
                var cacheKey = "moduleFunctionData";
                if (!_cache.TryGetValue(cacheKey, out List<Function> moduleFunctions))
                {
                    moduleFunctions = _context.Functions.ToList();
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(30));
                    _cache.Set(cacheKey, moduleFunctions, cacheEntryOptions);
                }
                return Ok(moduleFunctions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving module functions");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("Details")]
        [HttpGet("GetModuleFunction")]
        public async Task<IActionResult> GetModuleFunctions()
        {
            // 1. Định nghĩa khóa cache
            var cacheKey = "AllModuleFunctionsDto";

            // 2. Thử lấy dữ liệu từ cache
            if (_cache.TryGetValue(cacheKey, out IEnumerable<ModuleFunctionDto> cachedModules))
            {
                // Nếu có trong cache, trả về ngay lập tức
                return Ok(cachedModules);
            }

            // 3. Nếu không có trong cache, thực thi truy vấn DB như ban đầu
            var moduleWithDetails = await _context.Modules
                .Include(m => m.Functions)
                .ThenInclude(f => f.RolePermissions)
                .ToListAsync();

            // map nó qua các dto
            var module = moduleWithDetails.Select(e => new ModuleFunctionDto
            {
                Id = e.Id,
                Name = e.Name,
                Functions = e.Functions.Select(f => new FunctionDto
                {
                    Id = f.Id,
                    ModuleId = f.ModuleId,
                    Name = f.Name,
                    PermissionDtos = f.RolePermissions.Select(rp => new PermissionDto
                    {
                        RoleId = rp.RoleId,
                        FunctionId = rp.FunctionId,
                        CanCreate = rp.CanCreate,
                        CanRead = rp.CanRead,
                        CanWrite = rp.CanUpdate,
                        CanDelete = rp.CanDelete
                    }).ToList()
                }).ToList()
            });

            // 4. Lưu kết quả vào cache trước khi trả về
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(30)); // Dữ liệu sẽ hết hạn sau 30 phút nếu không được truy cập lại

            _cache.Set(cacheKey, module, cacheEntryOptions);

            return Ok(module);
        }
        [HttpGet("getDetails")]
        public async Task<IActionResult> GetRoleWithDetailsPermission(string roleId)
        {
            //var cacheKey = $"RoleDetails_{roleId}";
            //if()
            try
            {
                var roleDetails = await _context.Modules // <-- Bắt đầu từ bảng Modules
    .Select(m => new Tree // <-- Tạo ra 1 Tree DTO cho MỖI Module tồn tại
    {
        Id = m.Id,
        Name = m.Name,
        Functions = m.Functions.Select(f => new FunctionSto
        {
            Id = f.Id,
            Name = f.Name,
            Permissions = f.RolePermissions
                .Where(rp => rp.RoleId == roleId) // <-- Lọc quyền chỉ cho roleId cụ thể
                .Select(rp => new Permissionst
                {
                    canCreate = rp.CanCreate,
                    canView = rp.CanRead, // Đã sửa canView cho khớp với Permissionst
                    canModify = rp.CanUpdate, // Đã sửa canModify cho khớp
                    canDelete = rp.CanDelete
                }).ToList()
        }).ToList()
    })
    .ToListAsync();

                if (roleDetails == null || roleDetails.Count == 0)
                {
                    return NotFound($"No permissions found for role ID: {roleId}");
                }
                return Ok(roleDetails);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving role permissions");
                return BadRequest(ex);
            }
        }
    }

}
