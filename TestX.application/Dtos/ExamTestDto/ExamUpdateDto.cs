using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestX.application.Dtos.ExamTestDto
{
    public class ExamUpdateDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int NumberOfQuestion { get; set; }
        [Required]
        public int SubjectId { get; set; }
        [Required]
        public int Time { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
