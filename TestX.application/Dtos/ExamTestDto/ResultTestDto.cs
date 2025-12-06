using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestX.application.Dtos.ExamTestDto
{
    public class ResultTestDto
    {
        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; }
        public int WrongAnswers { get; set; }
        public int QuestionSkip { get; set; }
        public double Score { get; set; }

        public TimeSpan TimeTaken { get; set; }
    }
}
