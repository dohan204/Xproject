using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestX.domain.Entities.AccountRole;

namespace TestX.domain.Entities.General
{
    public class StudentExam
    {
        public int Id { get; set; }
        public string AccountId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public int ExamId { get; set; }
        public Exam Exam { get; set; }
        public int? AttemptNo { get; set; }
        public int Score { get; set; }
        public int CorrectNumber { get; set; }
        public int TotalQuestion { get; set; }
        public bool IsPassed { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? CreateBy { get; set; }
        public string ? UpdateBy { get; set; }
        public int? WrongAnswer { get; set; }
        public ICollection<StudentExamDetails> StudentExamDetails { get; set; }
    }
}
