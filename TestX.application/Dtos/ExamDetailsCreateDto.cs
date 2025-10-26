using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestX.application.Dtos
{
    public class ExamDetailsCreateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int NumberOfQuestion { get; set; }
        public string SubjectName { get; set; }
        public int TimeTesting { get; set; }
        public ExamDetailsDto ExamDetailsDto { get; set; }
    }
}
