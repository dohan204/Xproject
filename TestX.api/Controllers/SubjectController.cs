using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using TestX.application.Repositories;
using TestX.domain.Entities.General;

namespace TestX.api.Controllers
{
    public class SubjectController : Controller
    {
        private readonly ISubjectRepository _subject; 
        private readonly ILogger<SubjectController> _logger;
        public SubjectController(ISubjectRepository subject, ILogger<SubjectController> logger)
        {
            _subject = subject;
            _logger = logger;
        }
        [HttpGet("subjects")]
        public async Task<IActionResult> GetSubject()
        {
            try
            {
                var subjects = await _subject.GetAllSubjects();
                _logger.LogInformation("lấy dữ liệu ra thành công.");
                return Ok(subjects);
            }
            catch (Exception ex)
            {
                _logger.LogError("sảy ra lỗi khi thực hiện lấy dữ liệu {error}", ex.Message);
                return NotFound(ex.Message);
            }
        }
        [HttpGet("subjects/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var subject = await _subject.GetSubjectById(id);
                if (subject == null)
                    return NotFound("không tìm thấy Môn học");
                _logger.LogInformation($"môn học với id: {id} đã tìm thấy hahaah");
                return Ok(subject);
            }
            catch (Exception ex)
            {
                _logger.LogError("Xảy ra lỗi khi thực hiện truy suất cơ sở dữ liệu: {error}", ex.InnerException);
                return NotFound(ex.Message);
            }
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create(Subject subject)
        {
            try
            {
                var sub = await _subject.AddAsync(subject);
                return Content("Thêm mới thành công.");
            }
            catch (DbException ex)
            {
                _logger.LogError("Sảy rra lỗi khi thực hiện thêm mới: {error}", ex.InnerException);
                return StatusCode(500, ex.Message);
            }
            catch(DbUpdateException ex)
            {
                _logger.LogError("Lỗi khi thêm mới: {error}", ex.InnerException);
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPut("Update_subject")]
        public async Task<IActionResult> Update(Subject subject, int id)
        {
            try
            {
                var update = await _subject.UpdateAsync(subject, id);
                return Content("Cập nhật thành công.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError("xảy ra lỗi khi thực hiện thao tác với dữ liệu {error}", ex.InnerException);
                return StatusCode(500, ex.Message);
            }
            catch(DbException ex)
            {
                _logger.LogError("lỗi: {error}", ex.InnerException);
                return StatusCode(500, ex.Message);
            }
        }
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> delete(int id)
        {
            try
            {
                var delete = await _subject.DeleteAsync(id);
                return Content("Xóa Môn thành công.");
            }
            catch (Exception ex)
            {
                _logger.LogError("thao tác không thành không. {error}", ex.Message);
                return StatusCode(500, ex.Message);
            }
        }
    }
}
