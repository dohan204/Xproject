using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestX.domain.Entities.General
{
    public class StudentExamDetails
    {
        public int Id { get; set; }
        public int StudentExamId { get;set; }
        public StudentExam StudentExam { get; set; }
        public int QuestionId { get; set; }
        public string StudentAnswer { get; set; }
        public int HistoryId { get; set; }
        public History History { get; set; }
        public int IsCorrect { get; set; }
        public DateTime? CreateAt { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedAt { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }
}
