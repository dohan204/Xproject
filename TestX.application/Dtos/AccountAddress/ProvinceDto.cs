using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestX.application.Dtos.AccountAddress
{
    public class ProvinceDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public WardsCommuneDto WardsCommune { get; set; }
    }
}
