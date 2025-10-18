using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestX.application.Dtos.AccountAddress
{
    public class WardsCommuneDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int ProvinceId { get; set; }
        public ProvinceDto Province { get; set; }
    }
}
