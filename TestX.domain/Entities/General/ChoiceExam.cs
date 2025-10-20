using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestX.domain.Entities.AccountRole;

namespace TestX.domain.Entities.General
{
    public class ChoiceExam
    {
        public string AccountId { get; set; }
        public ApplicationUser User { get; set; }
        public int ExamId { get; set; }
        public Exam Exam { get; set; }
        public string ContentExam { get; set; }
        public string Description { get; set;}
        public int HistoryId { get; set; }
        public History History { get; set; }
    }
}
