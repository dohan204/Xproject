using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestX.application.Dtos.AccountAddress;
using TestX.domain.Entities.AccountRole;

namespace TestX.application.Dtos.Role
{
    public class UpdateRoleDto
    {
        //public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime ModifiedAt { get; set; }
        public bool Active { get; set; }
        public ICollection<RolePermissionDto> RolePermissionDtos { get; set; }
    }
}
