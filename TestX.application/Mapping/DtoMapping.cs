using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestX.domain.Entities.AccountRole;

namespace TestX.application.Mapping
{
    public class RoleMapping : Profile 
    {
        public RoleMapping()
        {
            CreateMap<UpdateRoleDto, ApplicationRole>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.RoleId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.RoleName))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.Active == "true"))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt));
        }
    }
}
