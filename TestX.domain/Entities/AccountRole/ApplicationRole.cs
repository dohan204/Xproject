using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestX.domain.Entities.AccountRole
{
    public class ApplicationRole : IdentityRole<string>
    {
        public string Description { get; set; }
        public bool Active { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; }
        public ApplicationRole(){ } 
        public ApplicationRole(string roleName) : base(roleName)
        {
            Description = string.Empty;
            Active = true;
            CreatedAt = DateTime.UtcNow;
            RolePermissions = new List<RolePermission>();
        }
    }
}
