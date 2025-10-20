using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestX.application.Dtos.ProVinceDto;
using TestX.domain.Entities.AccountRole;

namespace TestX.application.Repositories
{
    public interface IProvinceService
    {
        Task<List<ProvinceDto>> GetAllProvincesAsync();
        Task<Province?> GetProvinceByIdAsync(int id);
    }
}
