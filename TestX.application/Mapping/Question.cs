using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestX.application.Dtos.Question;
using TestX.domain.Entities.General;

namespace TestX.application.Mapping
{
    public class QuestionMapping : Profile
    {
        public QuestionMapping()
        {
            CreateMap<Question, QuestionViewDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Subject.Name))
                .ForMember(dest => dest.LevelName, opt => opt.MapFrom(src => src.Level.LevelName))
                .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.QuestionType.TypeName))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.ModifiedAt));

            CreateMap<QuestionCreateDto, Question>()
                .ForMember(dest => dest.SubjectId, opt => opt.MapFrom(src => src.SubjectId))
                .ForMember(dest => dest.LevelId, opt => opt.MapFrom(src => src.LevelId))
                .ForMember(dest => dest.QuestionTypeId, opt => opt.MapFrom(src => src.QuestionTypeId))
                .ForMember(dest => dest.Answer, opt => opt.MapFrom(src => src.QuestionAnswer))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.QuestionContent))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreateAt));
            CreateMap<QuestionUpdateDto, Question>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(opt => opt.Id))
                .ForMember(dest => dest.SubjectId, opt => opt.MapFrom(src => src.SubjectId))
                .ForMember(dest => dest.LevelId, opt => opt.MapFrom(src => src.LevelId))
                .ForMember(dest => dest.QuestionTypeId, opt => opt.MapFrom(src => src.QuestionTypeId))
                .ForMember(dest => dest.Answer, opt => opt.MapFrom(src => src.QuestionAnswer))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.QuestionContent))
                .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => src.UpdatedAt));
        }
    }
}
