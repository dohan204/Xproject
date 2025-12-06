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
using TestX.domain.Entities.AccountRole;
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
            int numberOfExam = 1;
            int crementExam = numberOfExam++;
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
            // timf va xoa di bang con cua no truoc
            var studentExam = await _identityContext.StudentExams.Where(e => e.ExamId == id).ToListAsync();
            if(studentExam == null)
                throw new Exception("Không thể xóa bài thi vì đã có sinh viên tham gia");
             _identityContext.StudentExams.RemoveRange(studentExam);
            await _identityContext.SaveChangesAsync();

            // tim va xoa di choice exam
            var choiceExam = await _identityContext.Choices.Where(e => e.ExamId == id).ToListAsync();
            if (choiceExam == null)
                return 0;
            _identityContext.Choices.RemoveRange(choiceExam);
            await _identityContext.SaveChangesAsync();

            // find exam: cuooix cung
            var exam = await _identityContext.Exams.FirstOrDefaultAsync(e => e.Id == id);
            if (exam == null) return 0;
            _identityContext.Exams.Remove(exam);
            await _identityContext.SaveChangesAsync();
            return exam.Id;
            // use transaction to ensure consistency across related deletes

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
        public async Task<ResultTestDto> HandleDataSubmit(Dictionary<int, string> resultFromFE, int examId)
        {
            var userId = HttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var name = HttpContextAccessor.HttpContext?.User?.FindFirst("fullName")?.Value;
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("User is not authenticated.");

            int correctAnswers = 0;
            var studentAnswers = new List<string>();
            int wrong = 0;
            int questionSkip = 0;
            var listAnswerWrong = new List<string>();
            var exam = await GetDetailsWithQuestion(examId);
            if (exam == null || resultFromFE == null)
                throw new Exception("Exam not found or invalid submission data.");
            var examFrom = await _identityContext.Exams.FirstOrDefaultAsync(e => e.Id == examId);
            if (examFrom == null)
                throw new Exception("Khong tim thay nguoi dung");
            examFrom.NumberOfExam += examFrom.NumberOfExam.GetValueOrDefault() + 1;
            _identityContext.Exams.Update(examFrom);
            Console.WriteLine(_identityContext.Entry(examFrom).State);
            await _identityContext.SaveChangesAsync();
            // bắt transaction để rollback nếu có lỗi
            using var transaction = await _identityContext.Database.BeginTransactionAsync();
            try
            {
                // 1) History
                var history = new History
                {
                    Name = $"Exam_{examId}_User_{userId}",
                    Time = DateTime.Now
                };
                await _identityContext.Histories.AddAsync(history);
                await _identityContext.SaveChangesAsync(); // history.Id có giá trị

                // 2) StudentExam (cha)
                var studentExam = new StudentExam
                {
                    AccountId = userId,
                    ExamId = examId,
                    StartDate = DateTime.Now,
                    CreatedAt = DateTime.Now,
                    CreateBy = name ?? "System"
                };
                await _identityContext.StudentExams.AddAsync(studentExam);
                await _identityContext.SaveChangesAsync(); // studentExam.Id có giá trị
                
                // 3) ChoiceExam (nếu nghiệp vụ cần 1 bản cho cả bài)
                // kiểm tra tồn tại để tránh duplicate key nếu chạy lại
                var existingChoice = await _identityContext.Choices
                    .FirstOrDefaultAsync(c => c.AccountId == userId && c.ExamId == examId && c.HistoryId == history.Id);

                if (existingChoice == null)
                {
                    var choiceExam = new ChoiceExam
                    {
                        AccountId = userId,
                        ExamId = examId,
                        HistoryId = history.Id,
                        ContentExam = "Exam summary or whatever",
                        Description = "Exam taken"  
                    };
                    await _identityContext.Choices.AddAsync(choiceExam);
                    await _identityContext.SaveChangesAsync();
                }
                // 4) StudentExamDetails cho từng câu hỏi
                foreach (var question in exam.Question)
                {
                    if (resultFromFE.TryGetValue(question.Id, out var selectedAnswer))
                    {
                        if(selectedAnswer == null)
                        {
                            questionSkip++;
                        }
                        // chỉ tăng khi đúng
                        if (string.Equals(selectedAnswer, question.Answer, StringComparison.OrdinalIgnoreCase))
                        {
                            correctAnswers++;
                        } else
                        {
                            listAnswerWrong.Add($"Câu hỏi ID {question.Id} - Đáp án của bạn: {selectedAnswer} - Đáp án đúng: {question.Answer}");
                            wrong++;
                        }
                            studentAnswers.Add(selectedAnswer ?? string.Empty);
                        var detail = new StudentExamDetails
                        {
                            StudentExamId = studentExam.Id,       // <-- phải là id của StudentExam đã tạo
                            QuestionId = question.Id,
                            StudentAnswer = selectedAnswer,
                            HistoryId = history.Id,
                            IsCorrect = string.Equals(selectedAnswer, question.Answer, StringComparison.OrdinalIgnoreCase) ? 1 : 0,
                            CreateAt = DateTime.Now
                        };

                        await _identityContext.StudentExamDetails.AddAsync(detail);
                    }
                }

                await _identityContext.SaveChangesAsync();

                // 5) Tính điểm và lưu vào StudentExam (update)
                int maxScore = 10;
                double score = exam.NumberOfQuestions > 0
                    ? (double)correctAnswers / exam.NumberOfQuestions * maxScore
                    : 0.0;

                var roundedScore = (int)Math.Round(score); // hoặc Math.Ceiling / Floor tuỳ m muốn

                // cập nhật studentExam
                studentExam.Score = roundedScore;
                studentExam.CorrectNumber = correctAnswers;
                studentExam.TotalQuestion = exam.NumberOfQuestions;
                studentExam.WrongAnswer = wrong;
                studentExam.IsPassed = score > 6; // tuỳ threshold
                                                  // nếu muốn lưu thời gian hoàn thành có thể thêm FinishDate

                _identityContext.StudentExams.Update(studentExam);
                await _identityContext.SaveChangesAsync();

                await transaction.CommitAsync();
                var resultTestDto = new ResultTestDto
                {
                    TotalQuestions = exam.NumberOfQuestions,
                    CorrectAnswers = correctAnswers,
                    Score = roundedScore,
                    //AnswersGiven = studentAnswers,
                    WrongAnswers = wrong,
                    QuestionSkip = questionSkip,
                };
                return resultTestDto;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task<int> AddExamFavorites(AddFavoriteExamDto favorite)
        {
            // đầu tiên lấy ra thông tin người dùng 
            var user = await _identityContext.Users.FirstOrDefaultAsync(u => u.Id == favorite.AccountId);
            if (user == null)
            {
                throw new Exception("không tìm thấy người dùng");
            }
            // lấy ra bài thi mà người dùng chọn 
            var examId = await _identityContext.Exams.FirstOrDefaultAsync(e => e.Id == favorite.ExamId);
            if(examId == null)
            {
                throw new Exception("Không tìm thấy bài thi");
            }
            // tạo đề thi yêu thích bằng các lấy thông tin từ 2 lần truy vấn trước 
            var existingFavorite = await _identityContext.ExamFavorites
                .FirstOrDefaultAsync(ef => ef.ExamId == favorite.ExamId);
            if(existingFavorite != null)
            {
                throw new Exception("Bài thi đã được thêm vào danh sách yêu thích");
            } else
            {
                var examFavorite = new ExamFavorite
                {
                    AccountId = user.Id,
                    ExamId = examId.Id,
                    CreatedAt = DateTime.Now
                };
                _identityContext.ExamFavorites.Add(examFavorite);
            }

            await _identityContext.SaveChangesAsync();
            return 1;
        }
        public async Task<int> RemoveFavorites(int Id)
        {
            var favorites = await _identityContext.ExamFavorites.FirstOrDefaultAsync(e => e.Id == Id);
            if (favorites == null)
                return 0;
            // Lấy Ra người dùng với id favorite
            var userAccount = await _identityContext.Users.FirstOrDefaultAsync(i => i.Id == favorites.AccountId);
            var examId = await _identityContext.Exams.FirstOrDefaultAsync(e => e.Id == favorites.ExamId);
            if(userAccount != null && examId != null)
            {
                // 
                _identityContext.ExamFavorites.Remove(favorites);
            }
            // thực hiện xóa bài thi yêu thích của người dùng
            await _identityContext.SaveChangesAsync();
            return 1;
        }
        public async Task<List<FavoriteExamViewDto>> GetFavoriteExamsByAccountId(string accountId)
        {
            //var favoriteExams = from ef in _identityContext.ExamFavorites
            //                    join e in _identityContext.Exams on ef.ExamId equals e.Id
            //                    join s in _identityContext.Subjects on e.SubjectId equals s.Id
            //                    where ef.AccountId == accountId
            //                    select new FavoriteExamViewDto
            //                    {
            //                        Id = ef.Id,
            //                        ExamName = e.Title,
            //                        SubjectName = s.Name,
            //                        TestingTime = e.TestingTime,
            //                        NumberOfQuestion = e.NumberOfQuestion,
            //                        CreatedAt = ef.CreatedAt
            //                    };
            //return await favoriteExams.ToListAsync();\\
            var favoriteExam = await _identityContext.ExamFavorites.Where(a => a.AccountId == accountId)
                .Include(e => e.Exam).ThenInclude(s => s.Subject)
                .ToListAsync();
            if(favoriteExam.Count == 0)
            {
                throw new Exception("Không tìm thấy bài thi yêu thích nào");
            }
            var favoriteExamDto = _mapper.Map<List<FavoriteExamViewDto>>(favoriteExam);
            return favoriteExamDto;
        }
        public async Task<List<ExamStudentView>> GetExamOfUser(string accountId)
        {
            var examCount = await _identityContext.StudentExams
                .Where(se => se.AccountId == accountId).Include(e => e.Exam).ThenInclude(s => s.Subject)
                .ToListAsync();
           var listExamDto = _mapper.Map<List<ExamStudentView>>(examCount);
            return listExamDto;
        }
        public async Task<ResultTestDto> OnSumbitDataFree(Dictionary<int, string> answers, int examId)
        {
            var examIdDb = await GetDetailsWithQuestion(examId);
            if(examIdDb == null)
            {
                throw new Exception("Khong tim thay bai thi");
            }
            int correctAnswer = 0;
            int wrongAnswer = 0;
            foreach(var question in examIdDb.Question)
            {
                if(answers.TryGetValue(question.Id, out var selectAnser))
                {
                    if(string.Equals(selectAnser, question.Answer)){
                        correctAnswer++;
                    } else
                    {
                        wrongAnswer++;
                    }
                }
            }
            var score = (double)correctAnswer / examIdDb.NumberOfQuestions * 10;
            return new ResultTestDto
            {
                CorrectAnswers = correctAnswer,
                WrongAnswers = wrongAnswer,
                Score = score,
                TotalQuestions = examIdDb.NumberOfQuestions
            };
        }
    }
}