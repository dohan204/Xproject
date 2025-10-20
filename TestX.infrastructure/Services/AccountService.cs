using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel;
//using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TestX.application.Dtos.AccountAddress;
using TestX.application.Repositories;
using TestX.domain.Entities.AccountRole;
using TestX.infrastructure.Identity;

namespace TestX.infrastructure.Services
{
    public class AccountService : IAccountService
    {
        private readonly IdentityContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountService> _logger;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        private readonly ITokenService _tokenService;
        public AccountService(IdentityContext context, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, IMapper mapper, ILogger<AccountService> logger, 
            IPasswordHasher<ApplicationUser> passwordHasher, ITokenService tokenService)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _logger = logger;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;

        }

        public async Task<List<AccountDto>> GetAllAccountUserAsync()
        {
            var userAccount = await _context.Users.Include(e => e.Province).ToListAsync();
            var accountDto = _mapper.Map<List<AccountDto>>(userAccount);
            return accountDto;
        }
        public async Task<AccountDto?> GetByIdAsync(string id)
        {

            //var account = await _context.Users.AsQueryable();
            var userAccount = await _context.Users.Include(e => e.Province).AsNoTracking().FirstOrDefaultAsync(acc => acc.Id == id);
            //var userAccount = await _context.Users
            //.Include(u => u.WardsCommune)
            //    .ThenInclude(c => c.Province)
            //.AsNoTracking()
            //.FirstOrDefaultAsync(u => u.Id == id);

            var accountDto = _mapper.Map<AccountDto>(userAccount);  
            return accountDto;
        }
        public async Task<int> CreateAsync(CreateAccountDto accountDto)
        {

            //if (await ExistsEmailAsync(accountDto.Email))
            //{
            //    _logger.LogError("Email này đã tồn tại.");
            //    return 0;
            //}
            var province = await _context.Provinces.FindAsync(accountDto.ProvinceId);
            if (province == null)
                _logger.LogError("Không tìm thấy tỉnh/thành phố với Id {provinceId}", accountDto.ProvinceId);

            var ward = await _context.WardsCommunes.FindAsync(accountDto.wardsCommuneId);
            if (ward == null)
                _logger.LogError("Không tìm thấy xã/phường với Id {wardsCommuneId}", accountDto.wardsCommuneId);
            //Province? province = null;
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = accountDto.UserName,
                FullName = accountDto.FullName,
                Email = accountDto.Email,
                PhoneNumber = accountDto.PhoneNumber,
                DateOfBirth = accountDto.DateOfBirth,
                ProvinceId = accountDto.ProvinceId,
                WardsCommuneId = accountDto.wardsCommuneId,
                CreatedAt = DateTime.Now
            };
            var users = _mapper.Map<ApplicationUser>(user);
            IdentityResult result = await _userManager.CreateAsync(users, accountDto.Password);
            if (!result.Succeeded)
            {
                _logger.LogError("lỗi không tạo được tài khoản {error}", string.Join(",", result.Errors.Select(er => er)));
                return 0;
            }    
            IdentityResult roleAssign = await _userManager.AddToRoleAsync(user, "User");
            return 1;
        }
        public async Task<int> UpdateAsync(string id,UpdateAccountDto user)
        {
            if (user == null)
                return 0;
            var acc = await _userManager.FindByIdAsync(id);
            if(acc == null) return 0;
                acc.UserName = user.UserName;
                acc.FullName = user.FullName;
                acc.Email = user.Email;
                acc.PhoneNumber = user.PhoneNumber;
            if (!string.IsNullOrWhiteSpace(user.Password))
                acc.PasswordHash = _passwordHasher.HashPassword(acc, user.Password);

                acc.DateOfBirth = user.DateOfBirth;
                acc.UpdatedAt = DateTime.Now;
            
            IdentityResult result = await _userManager.UpdateAsync(acc);
            return result.Succeeded ? 1 : 0;
        }
        public async Task<bool> DeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return false;
            IdentityResult result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }
        public async Task<bool> ExistsEmailAsync(string email)
        {
            var emailUser = await _userManager.FindByEmailAsync(email);
            return emailUser != null;
        }
        public async Task<UserDto> LoginAsync(LoginDto login)
        {
            var user = await _userManager.FindByNameAsync(login.UserName);
            if (user == null) throw new Exception("không tìm thấy tài khoản người dùng.");

            var result = await _signInManager.CheckPasswordSignInAsync(user, login.Password, false);
            if (!result.Succeeded) throw new Exception("Tài khoản hoặc mật khẩu không hợp lệ.");
            return new UserDto
            {
                Email = user.Email!,
                FullName = user.FullName,
                Token = _tokenService.CreateToken(user)
            };
        }
        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
        public async Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) throw new Exception("Không tìm thấy người dùng.");
            IdentityResult result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            if(!result.Succeeded)
            {
                _logger.LogError("Lỗi không thể đổi mật khẩu {error}", string.Join(",", result.Errors.Select(er => er)));
            }
            return result;
        }
    }
}
