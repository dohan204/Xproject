using Microsoft.AspNetCore.Mvc;
using TestX.application.Dtos.AccountAddress;
using TestX.application.Dtos;
using TestX.application.Repositories;
using TestX.api.CustomException;

namespace TestX.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {

        private readonly IAccountService _accountService;
        private readonly ILogger<AccountController> _logger;
        //private readonly TimeSpan _time;
        
        public AccountController(IAccountService accountService, ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _logger = logger;
            //_time = 
        }
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            //try
            //{
            //    var account = await _accountService.GetAllAccountUserAsync();
            //    return Ok(account);
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError("có lỗi sảy ra khi lấy dữ liệu {error}", ex.InnerException);
            //    throw new Exception("data not found.");
            //}
            var accounts = await _accountService.GetAllAccountUserAsync();
            if (accounts == null || !accounts.Any())
                throw new NotFoundException("Account", "danh sách người dùng rỗng");
            return Ok(accounts);
        }
        [HttpGet("getbyId")]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
           var account = await _accountService.GetByIdAsync(id);
            if (account == null)
                throw new NotFoundException("account", id);
            return Ok(account);
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateAccountDto accountDto)
        {
            var account = await _accountService.CreateAsync(accountDto);
            if (account == 0)

                throw new Exception("lỗi kh tạo được");
            return Content("Tạo người dùng thành Công.");
        }
        [HttpPut("updateAccount")]
        public async Task<IActionResult> UpdateAsync(string id, [FromBody] UpdateAccountDto accountDto)
        {
            //try
            //{
            //    var result = await _accountService.UpdateAsync(id, accountDto);
            //    if (result == 0)
            //        return NotFound();
            //    return Ok(result);
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError("có lỗi sảy ra khi cập nhật tài khoản {error}", ex.InnerException);
            //    throw new Exception("update account failed.");
            //}
            var result = await _accountService.UpdateAsync(id, accountDto);
            if (result == 0)
                throw new NotFoundException("account", id);
            return Content("Cập nhật tài khoản thành công.");
        }
        [HttpDelete("deleteAccount")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            //try
            //{
            //    var result = await _accountService.DeleteAsync(id);
            //    if (!result)
            //        return NotFound();
            //    return Ok(result);
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError("có lỗi sảy ra khi xóa tài khoản {error}", ex.InnerException);
            //    throw new Exception("delete account failed.");
            //}
            var result = await _accountService.DeleteAsync(id);
            if (!result)
                throw new NotFoundException("account", id);
            return Content("Xóa tài khoản thành công.");
        }
        [HttpPatch("ChangePassword")]
        public async Task<IActionResult> ChangePasswordAsync(string userId, [FromBody] ChangePasswordDto dto)
        {
            //try
            //{
            //    var result = await _accountService.ChangePasswordAsync(userId, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);
            //    if (!result.Succeeded)
            //        _logger.LogWarning("đổi mật khẩu thất bại cho {userId}: {error}",userId, string.Join(",", result.Errors.Select(er => er)));
            //    return Ok("Password changed successfully.");
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex, "Có lỗi xảy ra khi đổi mật khẩu cho user {UserId}", userId);
            //    return StatusCode(500, "Đã xảy ra lỗi trong quá trình đổi mật khẩu.");
            //}
            var result = await _accountService.ChangePasswordAsync(userId, dto.CurrentPassword, dto.NewPassword);

            if (!result.Succeeded)
                throw new ValidateException("Password", string.Join(", ", result.Errors.Select(e => e.Description)));

            return Ok("Password changed successfully.");

        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            try
            {
                var login = await _accountService.LoginAsync(loginDto);
                if (login == null)
                    return NotFound();
                return Ok(login);
            }
            catch (Exception )
            {
                _logger.LogError("lỗi.");
                throw;
            }
        }
    }
}
