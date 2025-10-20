using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestX.application.Dtos.AccountAddress
{
    public class RolePermissionDto
    {
        public int Id { get; set; }
        public string RoleId { get; set; }
        public int FunctionId { get; set; }
        public bool CanCreate { get; set; }
        public bool CanDelete { get; set; }
        public bool CanRead { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanModify { get; set; }
        public RolePermissionDto() { }
    }
}
