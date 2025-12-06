using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestX.application.Mapping.Exam
{
    public class AccountWithScoreExamDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string TitleExam { get; set; }
        public string SubjectName { get; set; }
        public int NumberOfQuestion { get; set; }
        public DateTime ExamDate { get; set; }
        public int Score { get; set; }
    }
}
