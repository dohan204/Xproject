using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
//using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestX.application.Dtos.Question;
using TestX.application.Repositories;
using TestX.infrastructure.Identity;
using TestX.domain.Entities.General;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;
using TestX.application;
using System.Xml;

namespace TestX.infrastructure.Services
{
    public class QuestionService : IQuestionService
    {
        //private readonly HttpContent _content;s
        private static readonly Random _random = new Random();
        private readonly IdentityContext _context;
        private readonly IMapper _mapper;
        public QuestionService(IdentityContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            //_content = content;
        }
        public async Task<List<QuestionViewDto>> AllQuestion()
        {
            var questions = await _context.Questions.
                ProjectTo<QuestionViewDto>(_mapper.ConfigurationProvider).ToListAsync();
            return questions;
        }
        public async Task<int> GetAllCountQuestion()
        {
                       var count = await _context.Questions.CountAsync();
            return count;

        }
        public async Task<QuestionViewDto> GetQuestion(int questionId)
        {
            var question = await _context.Questions.ProjectTo<QuestionViewDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(quest => quest.Id == questionId);
            return question!;
        }
        public async Task<int> CreateAsync(QuestionCreateDto questionCreateDto)
        {
            var question = _mapper.Map<Question>(questionCreateDto);
            question.CreatedAt = DateTime.Now;
            question.CreatedBy = "admin";
            await _context.Questions.AddAsync(question);
            await _context.SaveChangesAsync();
            return question.Id;
        }
        public async Task<int> UpdateAsync(QuestionUpdateDto questionUpdateDto)
        {
            var question = await _context.Questions.FindAsync(questionUpdateDto.Id);
            if (question == null)
                return 0;
            questionUpdateDto.UpdatedAt = DateTime.Now;
            _mapper.Map(questionUpdateDto, question);
            await _context.SaveChangesAsync();
            return questionUpdateDto.Id;
        }
        public async Task DeleteAsync(int id)
        {
            var question = await _context.Questions.FirstOrDefaultAsync(q => q.Id == id);
            if (question == null)
                return;
            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();
        }
        public async Task<List<QuestionViewDto>> RandomQuestionByLevel(int level,int subjectId, int numberOfQuestion)
        {
            //var questions = await _context.Questions.ToListAsync();
            var randomList = await _context.Questions
                .Where(sub => sub.SubjectId == subjectId && sub.LevelId == level)
                .OrderBy(question => Guid.NewGuid()) // query trên db giảm thiểu tốn bộ nhớ
                .Take(numberOfQuestion)
                .ToListAsync();
            if (randomList.Count < numberOfQuestion)
                throw new Exception("Số lượng câu hỏi cần lấy không đủ");
            var dtos = _mapper.Map<List<QuestionViewDto>>(randomList);
            return dtos;
        }
        public async Task<int> Delete(int id)
        {
            var question = await _context.Questions.AsNoTracking().FirstOrDefaultAsync(q => q.Id ==id);
            if (question == null) return 0;
            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();
            return 1;
        }
        public async Task<PagedResult<QuestionViewDto>> GetPagedQuestionById(int level, int subjectId, int pageSize = 10, int pageNumber = 1)
        {
            if(pageNumber < 1) pageNumber = 1;
            if(pageSize < 1) pageSize = 10;

            var query = _context.Questions.Where(s => s.SubjectId == subjectId && s.LevelId == level);

            var totalCount = await query.CountAsync(); // tổng số bản ghi thỏa mãn điều kiện được đưa ra 
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            if (totalPages > 0 && pageNumber > totalPages) pageNumber = totalPages;

            var questions = await query.OrderBy(s => s.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dtos = _mapper.Map<List<QuestionViewDto>>(questions);
            var result = new PagedResult<QuestionViewDto>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = dtos
            };
            return result;
        }
    }
}
