using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestX.application.Dtos.Question;
using TestX.domain.Entities.General;

namespace TestX.application.Dtos
{
    public class ExamDetailsDto
    {
        //public int Id { get; set; }
        public int ExamId { get; set; }
        public Exam Exam { get; set; }
        public int QuestionId { get; set; }
        public QuestionViewDto QuestionDto { get; set; }
    }
}
