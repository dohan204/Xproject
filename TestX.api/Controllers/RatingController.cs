using Microsoft.AspNetCore.Mvc;
using TestX.application.Repositories;

namespace TestX.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatingController : ControllerBase
    {
        private readonly RatingScoreSubject _rating;
        private readonly ILogger<RatingController> _logger;
        public RatingController(RatingScoreSubject rating, ILogger<RatingController> logger)
        {
            _rating = rating;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var res = await _rating.GetExamAndRating();
                if (res == null)
                    return NotFound();
                _logger.LogInformation("lấy dữ liệu thành công");
                return Ok(res);
            } catch (Exception ex)
            {
                _logger.LogError("Xảy ra lỗi:{ex} ", ex);
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("Rank")]
        public async Task<IActionResult> GetExamRank()
        {
            try
            {
                var data = await _rating.GetAccountWithScoresAsync();
                if (data == null)
                    return NotFound();
                _logger.LogInformation("Lấy dữ liệu thành công");
                return Ok(data);
            } catch (Exception ex)
            {
                _logger.LogError("Xảy ra lỗi trong quá trình xử lý: {ex}", ex);
                return NotFound();
            }
        }
    }
}
