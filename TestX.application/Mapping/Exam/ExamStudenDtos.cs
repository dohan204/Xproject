using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestX.application.Mapping.Exam
{
    public class ExamStudenDtos : Profile
    {
        public ExamStudenDtos()
        {
            CreateMap<TestX.domain.Entities.General.StudentExam, TestX.application.Dtos.ExamTestDto.ExamStudentView>()
                .ForMember(dest => dest.examId, opt => opt.MapFrom(src => src.ExamId))
                .ForMember(dest => dest.examName, opt => opt.MapFrom(src => src.Exam.Title))
                .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Exam.Subject.Name))
                .ForMember(dest => dest.QuestionQuantity, opt => opt.MapFrom(src => src.TotalQuestion))
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Exam.TestingTime))
                .ForMember(dest => dest.IsCorrect, opt => opt.MapFrom(src => src.CorrectNumber))
                .ForMember(dest => dest.IsWrong, opt => opt.MapFrom(src => src.WrongAnswer))
                .ForMember(dest => dest.Score, opt => opt.MapFrom(src => src.Score))
                .ForMember(dest => dest.ExamDate, opt => opt.MapFrom(src => src.CreatedAt));
        }
    }
}
