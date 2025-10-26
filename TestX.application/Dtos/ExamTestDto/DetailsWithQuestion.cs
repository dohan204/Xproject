using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestX.application.Dtos.ExamTestDto
{
    public class ExamWithQuestion
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SubjectName { get; set; }
        public int NumberOfQuestions { get; set; }
        public int TimeTest { get; set; }
        public List<QuestionWithExamDto> Question { get; set; }

    }
}
