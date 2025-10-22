using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Identity.Client;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using TestX.application.Dtos.School;
using TestX.application.Repositories;
using TestX.infrastructure.Identity;

namespace TestX.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SchoolController : ControllerBase
    {
        private readonly ILogger<SchoolController> _logger;
        private readonly ISchoolService _schoolService;
        private readonly IdentityContext _context;
        public SchoolController(ISchoolService schoolService, ILogger<SchoolController> logger, IdentityContext context)
        {
            _schoolService = schoolService;
            _logger = logger;
            _context = context;
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
        //public async Task<IActionResult> f(int id)
        //{
        //    try
        //    {
        //        var school = await _schoolService.
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
        [HttpPost("Add_School")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CreateSchoolDto dto)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var school = await _schoolService.AddAsync(dto);
                return Ok(school);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError("Say ra loi khi thuc hien cap them du lieu. {errror}", ex.InnerException);
                return BadRequest(dto);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return BadRequest(dto);
            }
        }
        [HttpPut("Update-School")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateSchoolDto dto, int id)
        {
            if (!ModelState.IsValid)
                return BadRequest("yeu cau khong hop le");
            var school = _context.School.AsNoTracking().FirstOrDefaultAsync(sc => sc.Id == id);
            if (school == null)
                return NotFound($"Khong tim thay truong voi id la: {id}");
            try
            {
                var schoolUpdate = await _schoolService.UpdateAsync(dto, school.Id);
                return Ok(schoolUpdate);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError($"có lỗi sảy ra khi thực hiện cập nhật cơ sở dữ liệu.{ex.InnerException}");
                return StatusCode(500, ex.Message);
            }
             
            catch(Exception ex)
            {
                _logger.LogError($"Sảy ra lỗi với cơ sở dữ liệu.{ex.InnerException}");
                return StatusCode(500, ex.Message);
            } 
        }
        [HttpDelete("deleteSchool")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var school = await _schoolService.DeleteAsync(id);
                return Ok($"Xóa thành công trường với id: {id}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Sảy ra lỗi khi thực hiện thao tác xóa dữ liệu: {ex.InnerException}");
                return StatusCode(500, ex.Message);
            }
        }
    }
}
