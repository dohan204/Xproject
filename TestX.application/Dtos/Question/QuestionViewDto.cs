using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestX.application.Dtos.Question
{
    public class QuestionViewDto
    {
        public int Id { get; set; }
        public string SubjectName { get; set; }
        public string LevelName { get; set; }
        public string TypeName { get; set; }
        public string Content { get; set; }
        public string Answer { get; set;}
        public string OptionA { get; set; }
        public string OptionB { get; set; }
        public string OptionC { get; set; }
        public string OptionD { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = "admin";
        public DateTime UpdatedAt { get; set; } 
        public string UpdatedBy { get; set; } = "admin";
    }
}
