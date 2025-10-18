using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestX.application.Dtos.AccountAddress
{
    public class AccountDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int ProvinceId { get; set; } 
        public string ProvinceName { get; set; }
        public string WardsCommuneName { get; set; }

    }
}
