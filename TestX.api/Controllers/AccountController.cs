using Microsoft.AspNetCore.Mvc;
using TestX.application.Dtos.AccountAddress;
using TestX.application.Dtos;
using TestX.application.Repositories;

namespace TestX.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<AccountController> _logger;
        public AccountController(IAccountService accountService, ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var account = await _accountService.GetAllAccountUserAsync();
                return Ok(account);
            }
            catch (Exception ex)
            {
                _logger.LogError("có lỗi sảy ra khi lấy dữ liệu {error}", ex.InnerException);
                throw new Exception("data not found.");
            }
        }
        [HttpGet("getbyId")]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            try
            {
                var account = await _accountService.GetByIdAsync(id);
                if (account == null)
                    return NotFound();
                return Ok(account);
            }
            catch (Exception ex)
            {
                _logger.LogError("có lỗi sảy ra khi lấy dữ liệu theo id {error}", ex.InnerException);
                throw new Exception("data not found.");
            }
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateAccountDto accountDto)
        {
            try
            {
                var result = await _accountService.CreateAsync(accountDto);
                if (result == 0)
                    return BadRequest("Email đã tồn tại.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("có lỗi sảy ra khi tạo tài khoản {error}", ex.InnerException);
                throw new Exception("create account failed.");
            }
        }
        [HttpPut("updateAccount")]
        public async Task<IActionResult> UpdateAsync(string id, [FromBody] UpdateAccountDto accountDto)
        {
            try
            {
                var result = await _accountService.UpdateAsync(id, accountDto);
                if (result == 0)
                    return NotFound();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("có lỗi sảy ra khi cập nhật tài khoản {error}", ex.InnerException);
                throw new Exception("update account failed.");
            }
        }
        [HttpDelete("deleteAccount")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            try
            {
                var result = await _accountService.DeleteAsync(id);
                if (!result)
                    return NotFound();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("có lỗi sảy ra khi xóa tài khoản {error}", ex.InnerException);
                throw new Exception("delete account failed.");
            }
        }
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePasswordAsync(string userId, [FromBody] ChangePasswordDto changePasswordDto)
        {
            try
            {
                var result = await _accountService.ChangePasswordAsync(userId, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }
                return Ok("Password changed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError("có lỗi sảy ra khi đổi mật khẩu {error}", ex.InnerException);
                throw new Exception("change password failed.");
            }
        }
    }
}
