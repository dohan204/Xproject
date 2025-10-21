using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestX.domain.Entities.General
{
    public class Exam
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int NumberOfQuestion { get; set; }
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
        public int PassingMark { get; set; }
        public int TestingTime { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public ICollection<StudentExam> StudentExams { get; set; }
        public ICollection<ExamDetails> ExamDetails { get; set; }
    }
}
