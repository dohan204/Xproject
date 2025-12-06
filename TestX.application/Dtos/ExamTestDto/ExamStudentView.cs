using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestX.application.Dtos.ExamTestDto
{
    public class ExamStudentView
    {
        public int examId { get; set; }
        public string examName { get; set; }
        public string SubjectName { get; set; }
        public int QuestionQuantity { get; set; }
        public int Duration { get; set; }
        public int IsCorrect { get; set; }
        public int IsWrong { get; set; }
        public int Score { get; set; }
        public DateTime ExamDate { get; set; }
    }
}
