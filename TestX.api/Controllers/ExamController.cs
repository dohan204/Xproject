using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.ComponentModel.DataAnnotations;
using TestX.application.Dtos.ExamTestDto;
using TestX.application.Repositories;

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
                return Ok(score);
            }
            catch (Exception ex)
            {
                _logger.LogError("lỗi khi nộp bài thi: {error}", ex.InnerException);
                return NotFound(ex.Message);
            }
        }
    }
}
