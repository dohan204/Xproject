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
    public class Schools : Profile
    {
        public Schools()
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
            CreateMap<School, SchoolDto>()
                .ForMember(dest => dest.NameLevel, opt => opt.MapFrom(src => src.SchoolLevel.LevelName));
            CreateMap<CreateSchoolDto, School>()
                .ForMember(dest => dest.SchoolCode, opt => opt.MapFrom(src => src.Code));
            CreateMap<UpdateSchoolDto, School>()
                .ForMember(dest => dest.SchoolCode, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => src.ModifiedAt));
        }
    }
}
