using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestX.application.Dtos.ExamTestDto
{
    public class ExamCreateDto
    {
        [Required(ErrorMessage = "Tên là bắt buộc")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Tên môn thi là bắt buộc")]
        public int SubjectId { get; set; }
        public int NumberOfQuestion { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime ExamDate { get; set; }
        [Range(0, int.MaxValue)]
        public int Time { get; set; } = 50; // mặc định là 50 phút
        public DateTime CreatedAt { get; set; } 
    }
}
