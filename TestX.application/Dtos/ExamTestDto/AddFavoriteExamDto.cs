using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestX.application.Dtos.ExamTestDto
{
    public class AddFavoriteExamDto
    {
        public int Id { get; set; }
        [Required]
        public string AccountId { get; set; }
        [Required]
        public int ExamId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
