using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestX.domain.Entities.General
{
    public class Level
    {
        public int Id { get; set; }
        public string LevelName { get; set; }
        //public ICollection<School> Schools { get; set; }
        //public Level()
        //{
        //    Schools = new List<School>();
        //}
        public ICollection<Question> Questions { get; set; }
    }
}
