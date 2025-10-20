using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestX.application.Dtos.School;
using TestX.domain.Entities.General;

namespace TestX.application.Mapping
{
    public class School : Profile
    {
        public School()
        {
            CreateMap<SchoolLevel, SchoolLevelDto>()
                .ForMember(s => s.Schools, opt => opt.MapFrom(src => src.Schools != null
                ? src.Schools.Select(s => new SchoolDto
                {
                    //Id = s.Id,
                    Name = s.Name,
                    Address = s.Address,
                    SchoolLevelId = s.SchoolLevelId,
                    NumberPhone = s.PhoneNumber,
                    Email = s.Email,
                    SchoolCode = s.SchoolCode
                }).ToList() : new List<SchoolDto>()));
        }
    }
}
