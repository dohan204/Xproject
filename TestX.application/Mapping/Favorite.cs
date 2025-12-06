using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestX.application.Dtos.ExamTestDto;
using TestX.domain.Entities.AccountRole;

namespace TestX.application.Mapping
{
    public class Favorite : Profile
    {
        public Favorite()
        {
            CreateMap<ExamFavorite, FavoriteExamViewDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ExamId, opt => opt.MapFrom(src => src.ExamId))
                .ForMember(dest => dest.ExamName, opt => opt.MapFrom(src => src.Exam.Title))
                .ForMember(dest => dest.QuestionQuantity, opt => opt.MapFrom(src => src.Exam.NumberOfQuestion))
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Exam.TestingTime))
                .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Exam.Subject.Name))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedAt));
        }
    }
}
