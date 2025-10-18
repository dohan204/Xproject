using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestX.domain.Entities.AccountRole
{
    public class Module
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Function> Functions { get; set; } = new List<Function>();
        public Module(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}
