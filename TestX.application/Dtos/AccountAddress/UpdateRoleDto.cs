using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestX.application.Dtos.AccountAddress;

namespace TestX.domain.Entities.AccountRole
{
    public class UpdateRoleDto
    {
        public string RoleName { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<RolePermissionDto> RolePermissions { get; set; } 
    }
}
