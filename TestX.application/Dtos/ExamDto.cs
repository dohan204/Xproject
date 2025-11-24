using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestX.domain.Entities.General;

namespace TestX.application.Dtos
{
    public class ExamDto
    {
        public string Title { get; set; }
        public int NumberOfQuestion { get; set; }
        public int SubjectId { get; set; }
        public TestX.domain.Entities.General.Subject Subject { get; set; } // Fully qualify to avoid ambiguity
        public int PassingMark { get; set; }
        public int TestingTime { get; set; }
        public DateTime ExamDate { get; set; }
    }
}
