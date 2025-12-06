using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.ComponentModel.DataAnnotations;
using TestX.application.Dtos.ExamTestDto;
using TestX.application.Repositories;
using TestX.domain.Entities.AccountRole;

namespace TestX.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExamController : ControllerBase
    {
        private readonly IExamRepository _examRepository;
        private readonly ILogger<ExamController> _logger;
        private readonly ICacheService _cacheService;
        public ExamController(IExamRepository examRepository, ILogger<ExamController> logger,
            ICacheService cache)
        {
            _examRepository = examRepository;
            _logger = logger;
            _cacheService = cache;
        }
        [HttpGet("exams")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var exams = await _examRepository.GetAllExamDetails();
                return Ok(exams);
            }
            catch (Exception ex)
            {
                _logger.LogError("Sảy ra lổi khi thực hiện truy suất: {error}", ex.InnerException);
                return NotFound(ex.Message);
            }
        }
        [HttpGet("countExam")]
        public async Task<IActionResult> CountExam()
        {
            try
            {
                var count = await _examRepository.GetExamCount();
                return Ok(count);
            }
            catch (Exception ex)
            {
                _logger.LogError("lỗi khi thực hiện truy suất: {error}", ex.InnerException);
                return NotFound(ex.Message);
                throw;
            }
        }
        [HttpGet("getNameExam")]
        public async Task<IActionResult> GetName(string name)
        {
            try
            {
                var examName = await _examRepository.GetExamByName(name);
                return Ok(examName);
            }
            catch (Exception ex)
            {
                _logger.LogError("lỗi khi thực hiện truy suất: {error}", ex.InnerException);
                return NotFound(ex.Message);
                throw;
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateExam(ExamCreateDto dto)
        {
            var exam = await _examRepository.CreateAsync(dto);
            if (exam == 0)
                return BadRequest("không ther tạo bài thi.");
            return Ok(exam);
        }
        [HttpPost("CreateWithquesition")]
        public async Task<IActionResult> Create(ExamCreateDto dto)
        {
            var exams = await _examRepository.CreateExamWithQuestion(dto);
            if (exams == 0)
                return BadRequest();
            return Ok(exams);
        }
        [HttpGet("examDetails/{id}")]
        public async Task<IActionResult> GetDetails(int id)
        {
            try
            {
                var details = await _examRepository.GetDetailsWithQuestion(id);
                if (details == null)
                    return NotFound();
                return Ok(details);
            }
            catch (Exception ex)
            {
                _logger.LogError("sảy ra lỗi khi cố thực hiện tao tác lấy dữ liệu: {error}", ex.InnerException);
                return NotFound(ex.Message);
            }
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var exam = _examRepository.Delete(id);
                if (exam is null)
                    return NotFound();
                return Ok("Xóa thành công.");
            }
            catch (Exception ex)
            {
                _logger.LogError("Lỗi,{error}", ex.Message);
                return StatusCode(500);
            }
        }
        [HttpGet("randomExam")]
        public async Task<IActionResult> GetRandomExam()
        {
            try
            {
                var exam = await _examRepository.GetRandomExam();
                return Ok(exam);
            }
            catch (Exception ex)
            {
                _logger.LogError("lỗi khi lấy dữ liệu ngẫu nhiên: {error}", ex.InnerException);
                return NotFound(ex.Message);
            }
        }
        [HttpGet("examBySubject")]
        public async Task<IActionResult> GetExamBySubjectName(int name)
        {
            try
            {
                var exam = await _examRepository.GetExamBySubjectName(name);
                return Ok(exam);
            }
            catch (Exception ex)
            {
                _logger.LogError("lỗi khi lấy dữ liệu theo tên môn học: {error}", ex.InnerException);
                return NotFound(ex.Message);
            }
        }
        [HttpGet("examBySubjectName")]
        public async Task<IActionResult> GetBySubject(string name)
        {
            try
            {
                var exam = await _examRepository.GetBySubject(name);
                return Ok(exam);
            }
            catch (Exception ex)
            {
                _logger.LogError("lỗi khi lấy dữ liệu theo tên môn học: {error}", ex.InnerException);
                return NotFound(ex.Message);
            }
        }
        [HttpPost("submitExam/{examId}")]
        public async Task<IActionResult> SubmitExam([FromBody] Dictionary<int, string> resultFromFE, [Required] int examId)
        {
            try
            {
                var score = await _examRepository.HandleDataSubmit(resultFromFE, examId);
                Console.WriteLine("AUTH HEADER: " + HttpContext.Request.Headers["Authorization"]);
                _logger.LogInformation("User submitted exam {examId} with score {score}", examId, score);
                return Ok(score);
            }
            catch (Exception ex)
            {
                _logger.LogError("lỗi khi nộp bài thi: {error}", ex.InnerException);
                return NotFound(ex.Message);
            }
        }
        [HttpPost("FavoriteExam")]
        public async Task<IActionResult> FavoriteExam([FromBody] AddFavoriteExamDto dto)
        {
            try
            {
                var result = await _examRepository.AddExamFavorites(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("lỗi khi thêm bài thi yêu thích: {error}", ex.InnerException);
                return NotFound(ex.Message);
            }
        }
        [HttpGet("GetFavoriteExams")]
        public async Task<IActionResult> GetFavoriteExams([FromQuery] string accountId)
        {
            try
            {
                var favoriteExams = await _examRepository.GetFavoriteExamsByAccountId(accountId);
                return Ok(favoriteExams);
            }
            catch (Exception ex)
            {
                _logger.LogError("lỗi khi lấy danh sách bài thi yêu thích: {error}", ex.InnerException);
                return NotFound(ex.Message);
            }
        }
        [HttpGet("GetExamOfUser")]
        public async Task<IActionResult> GetExamOfUser([FromQuery] string accountId)
        {
            try
            {
                var exams = await _examRepository.GetExamOfUser(accountId);
                return Ok(exams);
            }
            catch (Exception ex)
            {
                _logger.LogError("lỗi khi lấy danh sách bài thi của người dùng: {error}", ex.InnerException);
                return NotFound(ex.Message);
            }
        }
        [HttpPost("Submit")]
        public async Task<IActionResult> OnSumbitDataFree([FromBody] Dictionary<int, string> answers, [Required] int examId)
        {
            try
            {
                var result = await _examRepository.OnSumbitDataFree(answers, examId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("lỗi khi nộp bài thi miễn phí: {error}", ex.InnerException);
                return NotFound(ex.Message);
            }
        }
        [HttpDelete("DeleteFavorite")]
        public async Task<IActionResult> DeleteFa([FromQuery] int Id)
        {
            try
            {
                var f = _examRepository.RemoveFavorites(Id);
                _logger.LogInformation("Xóa Bài thi yêu thích thành công");
                return Ok(f);
            }
            catch (Exception ex)
            {
                {
                    _logger.LogError("Lỗi xử lý xóa bài thi yêu thích: {ex}", ex);
                    return NotFound(ex.Message);
                }
            }
        }
    }
}
