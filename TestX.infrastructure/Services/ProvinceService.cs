using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestX.application.Repositories;
using TestX.domain.Entities.AccountRole;
using TestX.infrastructure.Identity;

namespace TestX.infrastructure.Services
{
    public class ProvinceService : IProvinceService
    {
        private readonly IdentityContext _context;
        public ProvinceService(IdentityContext context)
        {
            _context = context;
        }
        public async Task<List<Province>> GetAllProvincesAsync()
        {
            return await _context.Provinces.Include(e => e.WardsCommune).AsNoTracking().ToListAsync();
        }
    }
}
