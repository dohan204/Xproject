using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestX.application.Dtos.School
{
    public class CreateSchoolDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Tên trường không được quá 100 ký tự.")]
        public string Name { get; set; }
        [Required]
        public int SchoolLevelId { get; set; }
        [EmailAddress]
        [Required(ErrorMessage = "Email là bắt buộc.")]
        public string Email { get; set; }
        [StringLength(150, ErrorMessage ="Địa chỉ không được quá 150 ykys tự.")]
        public string Address { get; set; }
        public string Code { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
    }
}
