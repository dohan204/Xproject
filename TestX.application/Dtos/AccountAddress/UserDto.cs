using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestX.application.Dtos.AccountAddress
{
    public class UserDto
    {
        public string userId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Token { get; set; }
        public string ReturnUrl { get; set; }
    }
}
