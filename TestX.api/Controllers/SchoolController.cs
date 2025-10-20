using Microsoft.AspNetCore.Mvc;
using TestX.application.Repositories;

namespace TestX.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SchoolController : ControllerBase
    {
        private readonly ILogger<SchoolController> _logger;
        private readonly ISchoolService _schoolService;
        public SchoolController(ISchoolService schoolService, ILogger<SchoolController> logger)
        {
            _schoolService = schoolService;
            _logger = logger;
        }
        [HttpGet("get-AllSchools")]
        public async Task<IActionResult> GetAllSchool()
        {
            try
            {
                var school = await _schoolService.GetSchoolLevelAsync();
                return Ok(school);
            }
            catch (Exception ex)
            {
                _logger.LogError("sảy ra lỗi khi thực hiện lấy dữ liệu: {error}", ex.InnerException);
                return NotFound(ex.Message);
            }
        }
    }
}
