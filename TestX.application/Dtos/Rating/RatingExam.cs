using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestX.application.Dtos.Rating
{
    public class RatingExamDto
    {
        public string SubjectName { get; set; }
        public string TitleExam { get; set; }
        public int NumberOfExam { get; set; }
        public int Rating {  get; set; }
    }
}
