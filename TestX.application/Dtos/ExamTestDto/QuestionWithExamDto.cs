using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestX.domain.Entities.General;

namespace TestX.application.Dtos.ExamTestDto
{
    public class QuestionWithExamDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string Answer { get; set; }
        public string OptionA { get; set; }
        public string OptionB { get; set; }
        public string OptionC { get; set; }
        public string OptionD { get; set; }
        public string? OptionE { get; set; }
        public string? OptionF { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
