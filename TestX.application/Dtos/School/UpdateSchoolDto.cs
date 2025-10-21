using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestX.application.Dtos.School
{
    public class UpdateSchoolDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SchoolLevelId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Code { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}
