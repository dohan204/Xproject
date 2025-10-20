using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TestX.application.Features.School.Commands.CreateSchool
{
    //IRequest<int> nghĩa là khi xử lý xong, nó sẽ trả về int (ID của School vừa tạo).
    public class CreateSchoolCommand : IRequest<int>
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Code { get; set; }
        public int levelId { get; set; }
    }
}
