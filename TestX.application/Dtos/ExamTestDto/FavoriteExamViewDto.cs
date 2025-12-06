using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestX.application.Dtos.ExamTestDto
{
    public class FavoriteExamViewDto
    {
        public int Id { get; set; }
        public int ExamId { get; set; }
        public string ExamName { get; set; }
        public string SubjectName { get; set; }
        public int QuestionQuantity { get; set; }
        public int Duration { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
