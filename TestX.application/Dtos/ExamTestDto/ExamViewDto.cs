using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestX.application.Dtos.ExamTestDto
{
    public class ExamViewDto
    {
        public int Id { get; set; }
        public string ExamName { get; set; }
        public int TestingTime { get; set; }
        public int NumberOfQuestion { get; set; }
        public string SubjectName { get; set; }
    }
}
