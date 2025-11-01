using Microsoft.AspNetCore.Mvc;
using TestX.application.Repositories;

namespace TestX.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WardsCommuneController : ControllerBase
    {
        private readonly IWardsCommuneService _wardsCommuneService;
        private readonly ILogger<WardsCommuneController> _logger;
        public WardsCommuneController(IWardsCommuneService wardsCommuneService, ILogger<WardsCommuneController> logger)
        {
            _wardsCommuneService = wardsCommuneService;
            _logger = logger;
        }
        [HttpGet("getallWardsCommune")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var response = await _wardsCommuneService.GetAll();
                _logger.LogInformation("lấy dữ liệu thành công.");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Xảy ra lỗi khi thực hiện lấy dữ liệu: {error} .", ex.InnerException);
                return NotFound(ex.Message);
                throw;
            }
        }
    }
}
