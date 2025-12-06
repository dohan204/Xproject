using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestX.domain.Entities.General;

namespace TestX.domain.Entities.AccountRole
{
    public class ExamFavorite
    {
        public int Id { get; set; }
        public string AccountId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public int ExamId { get; set; }
        public Exam Exam { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}
