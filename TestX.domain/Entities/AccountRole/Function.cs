using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestX.domain.Entities.AccountRole
{
    public class Function
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ModuleId { get; set; }
        public Module Module { get; set; }
        public ICollection<AccountPermission> AccountPermissions { get; set; } = new List<AccountPermission>();
        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();  
        public Function(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}
