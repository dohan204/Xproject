using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestX.domain.Entities.AccountRole;
using TestX.application.Dtos.Role;

namespace TestX.application.Mapping.Exam
{
    public class RoleMapping : Profile
    {
        public RoleMapping()
        {
            CreateMap<UpdatePermisstion, RolePermission>()
                .ForMember(dest => dest.FunctionId, opt => opt.MapFrom(src => src.FunctionId))
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
                .ForMember(dest => dest.CanCreate, opt => opt.MapFrom(src => src.CanCreate))
                .ForMember(dest => dest.CanDelete, opt => opt.MapFrom(src => src.CanDelete))
                .ForMember(dest => dest.CanRead, opt => opt.MapFrom(src => src.CanRead))
                .ForMember(dest => dest.CanModify, opt => opt.MapFrom(src => src.CanWrite));
        }
    }
}
