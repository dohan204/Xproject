using Microsoft.AspNetCore.Mvc;
using TestX.application.Repositories;

namespace TestX.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProvinceController : ControllerBase
    {
        private readonly ILogger<ProvinceController> _logger;
        private readonly IProvinceService _provinceService;
        public ProvinceController(IProvinceService provinceService, ILogger<ProvinceController> logger)
        {
            _provinceService = provinceService;
            _logger = logger;
        }
        [HttpGet("get-AllProvinceWithCommune")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var provinces =  await _provinceService.GetAllProvincesAsync();
                return Ok(provinces);
            }
            catch (Exception ex)
            {
                _logger.LogError("có lỗi sảy ra khi lấy dữ liệu {error}", ex.InnerException);
                return BadRequest(ex.Message);
            }
        }
    }
}
