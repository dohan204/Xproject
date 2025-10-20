using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace TestX.domain.Entities.AccountRole
{
    public class Province
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public ICollection<WardsCommune> WardsCommune { get; set; } = new List<WardsCommune>();
        public ICollection<ApplicationUser> ApplicationUser { get; set; } 
        //public ICollection<District> Districts { get; set; }
        public Province(int Id,string name, string code)
        {
            this.Id = Id;
            this.Name = name;
            this.Code = code;
            ApplicationUser = new List<ApplicationUser>();
        }
    }
}
