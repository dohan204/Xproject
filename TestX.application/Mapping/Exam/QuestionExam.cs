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
    public class QuestionExam : Profile
    {
        public QuestionExam()
        {
            //CreateMap<Question, QuestionWithExamDto>()
            //    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            //    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            //    .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
            //    .ForMember(dest => dest.Answer, opt => opt.MapFrom(src => src.Answer));
            //CreateMap<Exam, ExamWithQuestion>()
            //    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            //    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Title))
            //    .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src =>
            //    src.ExamDetails.FirstOrDefault() != null
            //        ? src.ExamDetails.FirstOrDefault().Question.Subject.Name
            //        : string.Empty))
            //    .ForMember(dest => dest.TimeTest, opt => opt.MapFrom(src => src.TestingTime))
            //    .ForMember(dest => dest.NumberOfQuestions, opt => opt.MapFrom(src => src.NumberOfQuestion))
            //    .ForMember(dest => dest.Question, opt => opt.MapFrom(src => src.ExamDetails.Select(e => e.Question)));
        }
    }
}
