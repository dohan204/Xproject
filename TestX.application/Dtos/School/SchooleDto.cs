using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestX.application.Dtos.School
{
    public class SchoolDto
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string NumberPhone { get; set; }
        public string Email { get; set; }
        public string SchoolCode { get; set; }
        public int SchoolLevelId { get; set; }
        public SchoolLevelDto SchoolLevel { get; set; }
        public string NameLevel { get; set; }
    }
}
