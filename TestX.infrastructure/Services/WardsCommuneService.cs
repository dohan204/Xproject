using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
//using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestX.application.Dtos.AccountAddress;
using TestX.application.Repositories;
using TestX.domain.Entities.AccountRole;
using TestX.infrastructure.Identity;

namespace TestX.infrastructure.Services
{
    public class WardsCommuneService : IWardsCommuneService
    {
        private readonly IdentityContext _context;
        private readonly ICacheService _cacheService;
        private readonly TimeSpan _time;
        public WardsCommuneService(IdentityContext context, ICacheService cacheService, IConfiguration configuration)
        {
            _context = context;
            _cacheService = cacheService;
            _time = TimeSpan.FromMinutes(configuration.GetValue<int>("CacheSettings:Expiry"));
        }
        public async Task<List<WardsCommune>> GetAll()
        {
            var key = "List_wards";
            var cacheKey = await _cacheService.GetAsync<List<WardsCommune>>(key);
            if (cacheKey != null)
                return cacheKey;
            var listWardsCommune = await _context.WardsCommunes.AsNoTracking().ToListAsync();
            if (listWardsCommune != null)
                await _cacheService.SetAsync(key, listWardsCommune, _time);
            return listWardsCommune!;
        }
        public async Task<WardsCommune> GetById(int id)
        {
            return await _context.WardsCommunes.AsNoTracking().FirstOrDefaultAsync(w => w.Id == id);
        }
    }
}
