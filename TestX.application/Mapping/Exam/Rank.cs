using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestX.application.Dtos.Rating;
using TestX.domain.Entities.General;

namespace TestX.application.Mapping.Exam
{
    public class Rank : Profile
    {
        public Rank()
        {
            CreateMap<TestX.domain.Entities.General.Exam, RatingExamDto>()
                .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(srcc => srcc.Subject != null ? (srcc.Subject.Name) : string.Empty))
                .ForMember(dest => dest.TitleExam, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.NumberOfExam, opt => opt.MapFrom(src => src.NumberOfExam));

            CreateMap<StudentExam, AccountWithScoreExamDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.ApplicationUser.FullName))
                .ForMember(dest => dest.TitleExam, opt => opt.MapFrom(src => src.Exam.Title))
                .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Exam.Subject.Name))
                .ForMember(dest => dest.NumberOfQuestion, opt => opt.MapFrom(src => src.Exam.NumberOfQuestion))
                .ForMember(dest => dest.ExamDate, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.Score, opt => opt.MapFrom(src => src.Score));
        }
    }
}
