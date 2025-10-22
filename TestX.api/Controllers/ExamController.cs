using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.ComponentModel.DataAnnotations;
using TestX.application.Repositories;

namespace TestX.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExamController : ControllerBase
    {
        private readonly IExamRepository _examRepository;
        private readonly ILogger<ExamController> _logger;
        public ExamController(IExamRepository examRepository, ILogger<ExamController> logger)
        {
            _examRepository = examRepository;
            _logger = logger;
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
    }
}
