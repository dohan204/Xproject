using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestX.application.Dtos.Question
{
    public class QuestionCreateDto
    {
        [Required(ErrorMessage = "Môn là bắt buộc")]
        public int SubjectId { get; set; }
        [Required(ErrorMessage = "Kiểu câu hỏi là bắt buộc")]
        public int QuestionTypeId { get; set; }
        [Required(ErrorMessage = "Loại câu hỏi là bắt buộc.")]
        public int LevelId { get; set; }
        [Required]
        [StringLength(1000, ErrorMessage = "Câu hỏi không được quá 1000 từ.")]
        public string QuestionContent { get; set; } = null!;
        [Required(ErrorMessage = "Đáp án thì phải nhập chứ?")]
        public string QuestionAnswer { get; set; } = null!;
        [Required(ErrorMessage = "Không thể để trống ở trường này.")]
        [MaxLength(500, ErrorMessage = "Không được vượt quá 200 ký tự.")]
        public string OptionA { get; set; } = null!;
        [Required(ErrorMessage = "Không thể để trống ở trường này.")]
        [MaxLength(500, ErrorMessage = "Không được vượt quá 200 ký tự.")]
        public string OptionB { get; set; } = null!;
        [Required(ErrorMessage = "Không thể để trống ở trường này.")]
        [MaxLength(500, ErrorMessage = "Không được vượt quá 200 ký tự.")]
        public string OptionC { get; set; } = null!;
        [Required(ErrorMessage = "Không thể để trống ở trường này.")]
        [MaxLength(500, ErrorMessage = "Không được vượt quá 200 ký tự.")] 
        public string OptionD { get; set; } = null!;
        //public string OptionD { get; set; } = null!;
        public string? OptionE { get; set; }
        public string? OptionF { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
