using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestX.application.Dtos.ExamTestDto;
using TestX.domain.Entities.General;

namespace TestX.application.Mapping.Exams
{
    public class ExamMapping : Profile
    {
        public ExamMapping()
        {
            CreateMap<Exam, ExamViewDto>()
                .ForMember(dest => dest.ExamName, opt => opt.MapFrom(src => src.Title));

            CreateMap<Exam, ExamViewDetailsDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.TestingTime, opt => opt.MapFrom(src => src.TestingTime))
                .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Subject.Name));

            CreateMap<ExamCreateDto, Exam>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.SubjectId, opt => opt.MapFrom(src => src.SubjectId))
                .ForMember(dest => dest.TestingTime, opt => opt.MapFrom(src => src.Time));

            CreateMap<ExamUpdateDto, Exam>()
                .ForMember(dest => dest.TestingTime, opt => opt.MapFrom(src => src.Time));
        }
    }
}
