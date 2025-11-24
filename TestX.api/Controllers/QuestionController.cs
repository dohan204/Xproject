using Microsoft.AspNetCore.Mvc;
using TestX.application.Repositories;

using TestX.api.CustomException;
using TestX.application.Dtos.Question;
namespace TestX.api.Controllers
{
    [ApiController]
    [Route("api/Question")]
    public class QuestionController : ControllerBase
    {
        private Random random = new Random();
        private readonly IQuestionService _questionService;
        private readonly ILogger<QuestionController> logger;    
        public QuestionController(IQuestionService questionService, ILogger<QuestionController> logger)
        {
            _questionService = questionService;
            this.logger = logger;
        }
        [HttpGet("GetAllquestion")]
        public async Task<IActionResult> GetAll()
        {
            var questions = await _questionService.AllQuestion();
            if (questions is null)
                throw new NotFoundException("Account", questions!);
            logger.LogInformation("lấy thành công danh sách Câu hỏi.");
            return Ok(questions);
        }
        [HttpGet("question/{id}")]
        public async Task<IActionResult> GetById([FromQuery] int id)
        {
            var question = await _questionService.GetQuestion(id);
            if (question is null)
                throw new NotFoundException("Question", id);
            logger.LogInformation($"question có id: {question}");
            return Ok(question);
        }
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] QuestionCreateDto questionCreateDto)
        {
            if (!ModelState.IsValid)
                throw new InvalidOperationException();
            var question = await _questionService.CreateAsync(questionCreateDto);
            if (question is 0)
                throw new BusinessException("Question", "lỗi khi tạo dữ liệu");
            logger.LogInformation($"tạo thành công câu hỏi,{question}");
            return Content("Tạo câu hỏi thành công.");
        }
        [HttpPut("update_Question")]
        public async Task<IActionResult> Update([FromBody] QuestionUpdateDto questionUpdateDto)
        {
            if (!ModelState.IsValid)
                throw new InvalidOperationException();
            var question = await _questionService.UpdateAsync(questionUpdateDto);
            if (question is 0)
                throw new BusinessException("Question", "Sảy ra lỗi khi cập nhật dữ liệu");
            logger.LogInformation("Cập nhật dữ liệu thành công.");
            return Content("Cập nhật thành công.");
        }
        [HttpGet("Random_Question")]
        public async Task<IActionResult> RandomQuestion(int level, int number, int numberOfQuestion)
        {
            var listQuestions = await _questionService.RandomQuestionByLevel(level, number, numberOfQuestion);
            if (!listQuestions.Any())
                throw new NotFoundException("Question", listQuestions);
            logger.LogInformation($"Thông tin câu hỏi: {listQuestions}");
            return Ok(listQuestions);
        }

        [HttpGet("paged_Question")]
        public async Task<IActionResult> PagedQuestion(int level, int subject, int pageSize, int pageNumber)
        {
            var pagedQuestion = await _questionService.GetPagedQuestionById(level, subject, pageSize, pageNumber);
            if (pagedQuestion != null)
            {
                logger.LogError("Không tìm được dữ liệu.");
                throw new NotFoundException("Question", pagedQuestion);
            }
            logger.LogInformation("lấy ra thông tin thành công.");
            return Ok(pagedQuestion);
        }
        [HttpDelete("delete-Quesstion")]
        public async Task<IActionResult> Del(int id)
        {
            try
            {
                var quession = await _questionService.Delete(id);
                if (quession == 0)
                    return NotFound();
                return Content("Xóa thành công câu hỏi.");
            } catch (Exception ex)
            {
                logger.LogInformation("lỗi rồi: {error}", ex.InnerException);
                return BadRequest();
            }
        }
    }
}
