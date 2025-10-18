using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestX.domain.Entities.AccountRole
{
    public class WardsCommune
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProvinceId { get; set; }
        public Province Province { get; set; }
        public WardsCommune(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}
