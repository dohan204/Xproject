using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestX.domain.Entities.AccountRole;

namespace TestX.application.Repositories
{
    public interface IProvinceService
    {
        Task<List<Province>> GetAllProvincesAsync();
    }
}
