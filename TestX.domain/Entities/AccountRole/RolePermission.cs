using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestX.domain.Entities.AccountRole
{
    public class RolePermission
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string RoleId { get; set; }
        public ApplicationRole Role { get; set; }
        public int FunctionId { get; set; }
        public Function Function { get; set; }
        public bool CanCreate { get; set; }
        public bool CanRead { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }
        public bool CanModify { get; set; }
        public RolePermission() { }
        public RolePermission(int id, string name, string roleId, int functionId, bool canCreate, bool canRead, bool canUpdate, bool canDelete, bool canModify)
        {
            this.Id = id;
            this.Name = name;
            RoleId = roleId;
            FunctionId = functionId;
            CanCreate = canCreate;
            CanRead = canRead;
            CanUpdate = canUpdate;
            CanDelete = canDelete;
            CanModify = canModify;
        }
    }
}
