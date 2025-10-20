using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestX.application.Dtos.School
{
    public class SchoolLevelDto
    {
        public string LevelName { get; set; }
        public ICollection<SchoolDto> Schools { get; set; }
    }
}
