using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestX.domain.Entities.AccountRole;

namespace TestX.application.Repositories
{
    public interface IWardsCommuneService
    {
        Task<List<WardsCommune>> GetAll();
        Task<WardsCommune> GetById(int id);
    }
}
