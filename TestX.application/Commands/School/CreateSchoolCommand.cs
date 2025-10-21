using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestX.application.Commands.School
{
    public class CreateSchoolCommand : IRequest<int>
    {
        public string Name { get; set; }
        public int SchoolLevelId { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string SchoolCode { get; set; }

    }
}
