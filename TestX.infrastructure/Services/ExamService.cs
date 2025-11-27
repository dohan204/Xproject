using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

//using System.Data.Entity;
using System.Linq;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TestX.application.Dtos.ExamTestDto;
using TestX.application.Mapping.Exam;
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
        private static readonly IHttpContextAccessor HttpContextAccessor = new HttpContextAccessor();
        private readonly TimeSpan _time;
        private readonly Random _random = new Random();
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
        public async Task<int> GetExamCount()
        {
            var count = await _identityContext.Exams.CountAsync();
            return count;
        }
        public async Task<ExamViewDto> GetExamByName(string names)
        {
            var query = _identityContext.Exams.AsQueryable();
            if (!string.IsNullOrEmpty(names))
                query = query.Where(name => EF.Functions.Like(EF.Functions.Collate(name.Subject.Name, "SQL_Latin1_General_CP1_CI_AS"), $"%{names}%"));
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
        public async Task<ExamWithQuestion> GetRandomExam()
        {
            // biến đổi thành danh sách
            var randomExam = await _identityContext.Exams.ToListAsync();

            // lấy ngẫu nhiên một đề thi
            var randomIndex = _random.Next(0, randomExam.Count);

            // lấy ra đề thi đó bằng id và trả về chi tiết đề thi
            var randomExamId = randomExam[randomIndex];

            var examDetails = new ExamWithQuestion()
            {
                Id = randomExamId.Id,
                Name = randomExamId.Title,
                TimeTest = randomExamId.TestingTime,
                SubjectName = randomExamId.Subject?.Name ?? "Không lấy được tên môn thi",
                NumberOfQuestions = randomExamId.NumberOfQuestion,
                Question = await _identityContext.ExamDetails
                    .Where(ed => ed.ExamId == randomExamId.Id)
                    .Join(_identityContext.Questions,
                        ed => ed.QuestionId,
                        q => q.Id,
                        (ed, q) => new QuestionWithExamDto
                        {
                            Id = q.Id,
                            Content = q.Content,
                            Answer = q.Answer,
                            OptionA = q.OptionA,
                            OptionB = q.OptionB,
                            OptionC = q.OptionC,
                            OptionD = q.OptionD,
                            CreatedAt = q.CreatedAt,
                            ModifiedAt = q.ModifiedAt
                        })
                    .ToListAsync()
            };
            return examDetails;
        }
        public async Task<int> Delete(int id)
        {
            // remove cached exam details if present
            //var cacheKey = $"Question_Exam_{id}";
            //await _cacheService.RemoveAsync(cacheKey);

            // find exam
            var exam = await _identityContext.Exams.FirstOrDefaultAsync(e => e.Id == id);
            if (exam == null) return 0;

            // use transaction to ensure consistency across related deletes
            await using var transaction = await _identityContext.Database.BeginTransactionAsync();
            try
            {
                // remove exam details
                var examDetails = await _identityContext.ExamDetails
                    .Where(ed => ed.ExamId == id)
                    .ToListAsync();
                Console.WriteLine(examDetails);
                if (examDetails.Any())
                    _identityContext.ExamDetails.RemoveRange(examDetails);

                // remove choice exams (if any)
                var choiceExams = await _identityContext.Set<ChoiceExam>()
                    .Where(c => c.ExamId == id)
                    .ToListAsync();
                Console.WriteLine(choiceExams);
                if (choiceExams.Any())
                    _identityContext.Set<ChoiceExam>().RemoveRange(choiceExams);

                // remove student exams and their details (if any)
                var studentExams = await _identityContext.Set<StudentExam>()
                    .Where(se => se.ExamId == id)
                    .ToListAsync();
                if (studentExams.Any())
                {
                    var studentExamIds = studentExams.Select(se => se.Id).ToList();
                    var studentExamDetails = await _identityContext.Set<StudentExamDetails>()
                        .Where(d => studentExamIds.Contains(d.Id))
                        .ToListAsync();
                    if (studentExamDetails.Any())
                        _identityContext.Set<StudentExamDetails>().RemoveRange(studentExamDetails);

                    _identityContext.Set<StudentExam>().RemoveRange(studentExams);
                }

                // finally remove the exam itself
                _identityContext.Exams.Remove(exam);

                await _identityContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return id;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task<List<ExamViewDto>> GetExamBySubjectName(int id)
        {
            var examBySubject = from e in _identityContext.Exams
                                join s in _identityContext.Subjects on e.SubjectId equals s.Id
                                where s.Id == id
                                select new ExamViewDto
                                {
                                    Id = e.Id,
                                    ExamName = e.Title,
                                    TestingTime = e.TestingTime,
                                    NumberOfQuestion = e.NumberOfQuestion,
                                    SubjectName = s.Name
                                };
            return await examBySubject.ToListAsync();
        }
        public async Task<List<ExamViewDto>> GetBySubject(string name)
        {
            var query = _identityContext.Exams.AsQueryable();
            if (!string.IsNullOrEmpty(name))
                query = query.Where(sub => EF.Functions.Like(EF.Functions.Collate(sub.Subject.Code, "SQL_Latin1_General_CP1_CI_AS"), $"%{name}%"));
            var exam = await query
                .ProjectTo<ExamViewDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return exam;
        }
        public async Task<double> HandleDataSubmit(Dictionary<int, string> resultFromFE, int examId)
        {
            // laays ra id người dùng hiện tại từ token
            var userId = HttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            // kiểm tra nếu userId null thì ném ngoại lệ
            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }
            // biến đếm số câu trả lời đúng
            int correctAnswers = 0;
            // lấy ra đề thi cần để chấm điểm từ 
            var exam = await GetDetailsWithQuestion(examId);
            var dataFromFe = resultFromFE;
            if (exam == null || dataFromFe == null)
                return 0;
            var history = new History
            {
                Name = $"Exam_{examId}_User_{userId}",
                Time = DateTime.Now
            };

            await _identityContext.Set<History>().AddAsync(history);
            await _identityContext.SaveChangesAsync();
            // lập lặp qua danh sách câu hỏi trong đề thi và so sánh các đáp án 
            var correactAnswerList = new List<string>();
            foreach (var question in exam.Question)
            {
                // lấy ra câu hỏi dựa trên id, sau đó gán vào biến selectedAnswer
                if (dataFromFe.TryGetValue(question.Id, out var selectedAnswer))
                {
                    // so sánh đáp án được chọn với đáp án đúng trong đề 
                    if (string.Equals(selectedAnswer, question.Answer, StringComparison.OrdinalIgnoreCase))
                    {
                        correctAnswers++;
                        correactAnswerList.Add(selectedAnswer);
                    }
                    // lưu lại lựa chọn vào bảng ChoiceExam
                    var choiceExam = new ChoiceExam
                    {
                        AccountId = userId, // cần lấy id người dùng hiện tại
                        ExamId = examId,
                        HistoryId = history.Id, // cần
                        ContentExam = question.Content,
                        Description = $"Selected Answer: {selectedAnswer}"
                    };
                    await _identityContext.Set<ChoiceExam>().AddAsync(choiceExam);
                    var studentExamDetails = new StudentExamDetails
                    {
                        StudentExamId = userId, // tạm thời sử dụng hashcode của userId làm Id
                        QuestionId = question.Id,
                        StudentAnswer = selectedAnswer,
                        HistoryId = history.Id,
                        IsCorrect = string.Equals(selectedAnswer, question.Answer, StringComparison.OrdinalIgnoreCase) ? 1 : 0,
                        CreateAt = DateTime.Now
                    };
                }
            }
            await _identityContext.SaveChangesAsync();
            int maxScore = 10;
            // tính điểm dựa trên số câu đúng và tổng số câu hỏi
            double score = (double)correctAnswers / exam.NumberOfQuestions * maxScore;

            // sau khi đã chám điểm xong thì trả về số câu đúng 
            // lưu kết quả bài thi vào bảng StudentExam
            bool isPadded;
            if(score > 6)
            {
                isPadded = true;
                Console.WriteLine("Bạn đã vượt qua bài thi.");
            } else  {
                isPadded = false;
                Console.WriteLine("Bạn đã trượt bài thi.");
            }
            var studentExam = new StudentExam
                {
                    AccountId = userId,
                    ExamId = examId,
                    Score = (int)score,
                    IsPassed = isPadded,
                    CorrectNumber = correctAnswers,
                    TotalQuestion = exam.NumberOfQuestions,
                    StartDate = DateTime.Now,
                    CreatedAt = DateTime.Now
                };
            await _identityContext.Set<StudentExam>().AddAsync(studentExam);
            await _identityContext.SaveChangesAsync();
            return score;
        }
    }
}