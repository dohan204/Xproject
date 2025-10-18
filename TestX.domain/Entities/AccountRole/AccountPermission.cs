using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestX.domain.Entities.AccountRole
{
    public class AccountPermission
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AccountId { get; set; }
        public int FunctionId { get; set; }
    }
}
