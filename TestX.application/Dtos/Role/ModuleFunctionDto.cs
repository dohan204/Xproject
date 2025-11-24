using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestX.application.Dtos.Role
{
    public class ModuleFunctionDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<FunctionDto> Functions { get; set; } = new List<FunctionDto>();
    }
    public class FunctionDto
    {
        public int Id { get; set; }
        public int ModuleId { get; set; }
        public string Name { get; set; }
        public List<PermissionDto> PermissionDtos { get; set; } = new List<PermissionDto>();

    }
    public class PermissionDto
    {
        public int FunctionId { get; set; }
        public string RoleId { get; set; }
        public bool CanCreate { get; set; } = false;
        public bool CanDelete { get; set; } = false;
        public bool CanRead { get; set; } = false;
        public bool CanWrite { get; set; } = false;
    }
}
