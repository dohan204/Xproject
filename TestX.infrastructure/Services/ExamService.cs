using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
//using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestX.application.Dtos.ExamTestDto;
using TestX.application.Repositories;
using TestX.domain.Entities.General;
using TestX.infrastructure.Identity;

namespace TestX.infrastructure.Services
{
    public class ExamService : IExamRepository
    {
        private readonly IdentityContext _identityContext;
        private readonly IMapper _mapper;
        public ExamService(IdentityContext identityContext, IMapper mapper)
        {
            _identityContext = identityContext;
            _mapper = mapper;
        }
            public async Task<List<ExamViewDetailsDto>> GetAllExamDetails()
            {
            var examDto = await _identityContext.Exams
                .ProjectTo<ExamViewDetailsDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
                return examDto;
            }
        public async Task<ExamViewDto> GetExamByName(string names)
        {
            var query = _identityContext.Exams.AsQueryable();
            if (!string.IsNullOrEmpty(names))
                query = query.Where(name => EF.Functions.Like(EF.Functions.Collate(name.Title, "SQL_Latin1_General_CP1_CI_AS"), $"%{names}%"));
            var exam = await query.FirstOrDefaultAsync();
            var dtos = _mapper.Map<ExamViewDto>(exam);
            return dtos;
        }
        public async Task<int> CreateAsync(ExamCreateDto examCreateDto)
        {
            var exam = _mapper.Map<Exam>(examCreateDto);
            exam.CreatedAt = DateTime.Now;
            await _identityContext.Exams.AddAsync(exam);
            await _identityContext.SaveChangesAsync();
            return exam.Id;
        }
        public async Task<int> UpdateAsync(ExamUpdateDto examUpdateDto)
        {
            var existingExam = await _identityContext.Exams.FindAsync(examUpdateDto.Id);
            if (existingExam == null) return 0;
            _mapper.Map(examUpdateDto, existingExam);
            existingExam.ModifiedAt = DateTime.Now;
            await _identityContext.SaveChangesAsync();
            return existingExam.Id;
        }
    }
}
