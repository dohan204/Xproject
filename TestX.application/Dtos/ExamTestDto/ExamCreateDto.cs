using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestX.application.Dtos.ExamTestDto
{
    public class ExamCreateDto
    {
        public string Name { get; set; }
        public string NameSubject { get; set; }
        public DateTime ExamDate { get; set; }
        public int Time { get; set; }

    }
}
