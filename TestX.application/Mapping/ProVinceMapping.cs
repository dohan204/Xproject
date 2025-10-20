using AutoMapper;
using TestX.application.Dtos.ProVinceDto;
using TestX.domain.Entities.AccountRole;

namespace TestX.application.Mapping
{
    public class ProvinceMapping : Profile
    {
        public ProvinceMapping()
        {
            // Province -> ProvinceDto
            CreateMap<Province, ProvinceDto>()
                .ForMember(dest => dest.WardsDto,
                    opt => opt.MapFrom(src => src.WardsCommune != null
                        ? src.WardsCommune.Select(w => new WardsDto
                        {
                            //Id = w.Id,
                            Name = w.Name
                        }).ToList()
                        : new List<WardsDto>()
                    ));

            // ProvinceDto -> Province
            //CreateMap<ProvinceDto, Province>()
            //    .ForMember(dest => dest.WardsCommune,
            //        opt => opt.MapFrom(src => src.WardsDto != null
            //            ? src.WardsDto.Select(w => new WardsCommune
            //            {
            //                Id = w.Id,
            //                Name = w.Name,
            //                ProvinceId = src.Id
            //            }).ToList()
            //            : new List<WardsCommune>()
            //        ));
        }
    }
}
