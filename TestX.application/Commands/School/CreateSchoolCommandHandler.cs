using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestX.application.Repositories;

namespace TestX.application.Commands.School
{
    public class CreateSchoolCommandHandler : IRequestHandler<CreateSchoolCommand, int>
    {
        private readonly ISchoolService _schoolService;
        public CreateSchoolCommandHandler(ISchoolService schoolService)
        {
            _schoolService = schoolService;
        }
        public async Task<int> Handle(CreateSchoolCommand request, CancellationToken cancellationToken)
        {
            var schoolDto = new Dtos.School.SchoolDto
            {
                Name = request.Name,
                SchoolLevelId = request.SchoolLevelId,
                Address = request.Address,
                NumberPhone = request.PhoneNumber,
                Email = request.Email,
                SchoolCode = request.SchoolCode
            };
            //await _schoolService.AddAsync(schoolDto);
            return 1; // You might want to return the created school's ID or another relevant value
        }
    }
}
