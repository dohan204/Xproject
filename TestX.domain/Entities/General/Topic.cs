using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestX.domain.Entities.General
{
    public class Topic
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SubjectId { get; set; }
        public Subject Subject { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }

    }
}
