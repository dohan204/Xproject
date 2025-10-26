using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
//using System.Data.Entity;
using System.Linq;
using System.Net.WebSockets;
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
        private readonly ICacheService _cacheService;
        private readonly IdentityContext _identityContext;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly TimeSpan _time;
        public ExamService(IdentityContext identityContext, IMapper mapper, 
            ICacheService cacheService, IConfiguration configuration)
        {
            _identityContext = identityContext;
            _mapper = mapper;
            _cacheService = cacheService;
            _configuration = configuration;
            _time = TimeSpan.FromMinutes(configuration.GetValue<int>("CacheSettings:Expiry"));
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
        public async Task<int> CreateExamWithQuestion(ExamCreateDto dtos)
        {
            // lưu từ modle vào cơ sở dữ liệu 
            var exams = _mapper.Map<Exam>(dtos);
            exams.CreatedAt = DateTime.Now;
            await _identityContext.Exams.AddAsync(exams);
            await _identityContext.SaveChangesAsync();

            // lấy ra danh sách câu hỏi random theo môn cần thi
            var question = await _identityContext.Questions.AsNoTracking()
                .Where(sub => sub.SubjectId == dtos.SubjectId)
                .OrderBy(o => Guid.NewGuid().ToString())
                .Take(dtos.NumberOfQuestion)
                .ToListAsync();

            // tạo danh sách bài thi chi tiết mới 
            var questions = question.Select(e => new ExamDetails
            {
                ExamId = exams.Id,
                QuestionId = e.Id,
                CreatedAt = DateTime.Now,
            }).ToList();
            // lưu thay đổi vào cơ sở dữ lieuj.
            _identityContext.ExamDetails.AddRange(questions);
            await _identityContext.SaveChangesAsync();
            return exams.Id;  
        }
        public async Task<ExamWithQuestion?> GetDetailsWithQuestion(int id)
        {
            var key = $"Question_Exam_{id}";
            var cachedKey = await _cacheService.GetAsync<ExamWithQuestion>(key);
            if (cachedKey != null)
            {
                return cachedKey;
            }
            //var examDetails = await _identityContext.Exams
            //    .Include(e => e.ExamDetails)
            //        .ThenInclude(e => e.Question)
            //    .Include(e => e.Subject)
            //            .FirstOrDefaultAsync(e => e.Id == id);
            var examDetails = from e in _identityContext.Exams
                              join ed in _identityContext.ExamDetails on e.Id equals ed.ExamId
                              join q in _identityContext.Questions on ed.QuestionId equals q.Id
                              join s in _identityContext.Subjects on e.SubjectId equals s.Id
                              where (e.Id == id)
                              select new { Exam = e, Question = q, Subject = s };
            var groupbyDetails = await examDetails
                .GroupBy(x => new { x.Exam.Id, x.Exam.Title, x.Exam.TestingTime, x.Subject.Name, x.Exam.NumberOfQuestion })
                .Select(e => new ExamWithQuestion
                {
                    Id = e.Key.Id,
                    Name = e.Key.Title,
                    TimeTest = e.Key.TestingTime,
                    SubjectName = e.Key.Name,
                    NumberOfQuestions = e.Key.NumberOfQuestion,
                    Question = e.Select(g => new QuestionWithExamDto
                    {
                        Id = g.Question.Id,
                        Content = g.Question.Content,
                        Answer = g.Question.Answer,
                        OptionA = g.Question.OptionA,
                        OptionB = g.Question.OptionB,
                        OptionC = g.Question.OptionC,
                        OptionD = g.Question.OptionD,
                        CreatedAt = g.Question.CreatedAt,
                        ModifiedAt = g.Question.ModifiedAt
                    }).ToList()
                }).FirstOrDefaultAsync();
            if (groupbyDetails == null)
                return null;
            await _cacheService.SetAsync(key, groupbyDetails, _time);
            return groupbyDetails;
        }
    }
}
