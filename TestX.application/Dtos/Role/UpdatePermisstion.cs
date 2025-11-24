using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestX.application.Dtos.Role
{
    public class UpdatePermisstion
    {

        public int FunctionId { get; set; }
        public string Name { get; set; }
        public string RoleId { get; set; }
        public bool CanCreate { get; set; }
        public bool CanDelete { get; set; }
        public bool CanRead { get; set; }
        public bool CanWrite { get; set; }
    }
    public class Permission {
        
    }

}
