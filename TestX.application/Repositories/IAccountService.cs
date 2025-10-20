using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestX.application.Dtos.AccountAddress;
using TestX.domain.Entities;
using TestX.domain.Entities.AccountRole;

namespace TestX.application.Repositories
{
    public interface IAccountService
    {
        Task<List<AccountDto>> GetAllAccountUserAsync();
        Task<AccountDto?> GetByIdAsync(string id);
        Task<int> CreateAsync(CreateAccountDto user);
        Task<int> UpdateAsync(string id,UpdateAccountDto user);
        Task<bool> DeleteAsync(string id);
        //Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
        //Task<bool> ChangePasswordAsync(ApplicationUser user, string currentPassword, string newPassword);
        //string CreateToken(ApplicationUser user);
        Task<bool> ExistsEmailAsync(string email);
        Task<UserDto> LoginAsync(LoginDto login);
        Task LogoutAsync();
        Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
    }
}
