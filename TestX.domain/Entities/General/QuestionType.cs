using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestX.domain.Entities.General
{
    public class QuestionType
    {
        public int Id { get; set; }
        public string TypeName { get; set; }
        public ICollection<Question> Questions { get; set; }    
    }
}
