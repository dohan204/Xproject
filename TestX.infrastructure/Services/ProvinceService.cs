using System;
using System.Collections.Generic;
//using System.Data.Entity;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestX.application.Repositories;
using TestX.domain.Entities.AccountRole;
using TestX.infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using TestX.application.Dtos.ProVinceDto;

namespace TestX.infrastructure.Services
{
    public class ProvinceService : IProvinceService
    {
        private readonly IdentityContext _context;
        private readonly IMapper _mapper;
        public ProvinceService(IdentityContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<ProvinceDto>> GetAllProvincesAsync()
        {
            var provinces = await _context.Provinces.Include(e => e.WardsCommune)
                .AsNoTracking()
                .ToListAsync();
            var provinceDtos = _mapper.Map<List<ProvinceDto>>(provinces);
            return provinceDtos;
        }
        public async Task<Province?> GetProvinceByIdAsync(int id)
        {
            return await _context.Provinces.Include(e => e.WardsCommune)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
