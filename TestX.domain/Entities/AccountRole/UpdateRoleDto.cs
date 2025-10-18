using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestX.domain.Entities.AccountRole
{
    public class UpdateRoleDto
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public string Active { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
