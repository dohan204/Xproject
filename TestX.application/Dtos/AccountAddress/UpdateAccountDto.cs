using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestX.domain.Entities;

namespace TestX.application.Dtos.AccountAddress
{
    public class UpdateAccountDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public DateTime DateOfBirth { get; set; }
        public DateTime UpdatedAt { get; set; }
        //public ProvinceDto Province { get; set; }
        public int ProvinceId {get; set; }
        public int WardId { get; set; }
    }
}
