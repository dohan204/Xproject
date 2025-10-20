using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
//using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestX.application.Dtos.School;
using TestX.application.Repositories;
using TestX.infrastructure.Identity;

namespace TestX.infrastructure.Services
{
    public class SchoolService : ISchoolService
    {
        private readonly IdentityContext _context;
        private readonly IMapper _mapper;
        public SchoolService(IdentityContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<SchoolLevelDto>> GetSchoolLevelAsync()
        {
            var schools = await _context.SchoolLevel.Include(s => s.Schools).ToListAsync();
            //var schoolLevelDtos = schools.Select(sl => new SchoolLevelDto
            //{
            //    LevelName = sl.LevelName,
            //    Schools = sl.Schools.Select(s => new SchoolDto
            //    {
            //        Id = s.Id,
            //        SchoolName = s.SchoolName,
            //        Address = s.Address,
            //        SchoolLevelId = s.SchoolLevelId
            //    }).ToList()
            //}).ToList();
            var schoolDto = _mapper.Map<List<SchoolLevelDto>>(schools);
            return schoolDto;
        }
        public async Task<SchoolDto> FilterSchool(string schoolCode, string schoolName, string schoolId)
        {
            var query = _context.School.AsQueryable();

            if(!string.IsNullOrEmpty(schoolCode))
            {
                query = query.Where(s => EF.Functions.Like(s.SchoolCode, $"%{schoolCode}%"));
            }
            if (!string.IsNullOrEmpty(schoolName))
            {
                query = query.Where(s => EF.Functions.Like(s.Name, $"%{schoolName}%"));
            }
            if(!string.IsNullOrEmpty(schoolId) && int.TryParse(schoolId, out int id))
            {
                query = query.Where(s => s.Id == id);
            }
            var school = await query.FirstOrDefaultAsync();
            var schoolDto = _mapper.Map<SchoolDto>(school);
            return schoolDto;
        }
        public async Task<SchoolLevelDto> GetSchoolLevelAsync(string schoolLevelId)
        {
            var schoolLevel = await _context.SchoolLevel
                .Include(s => s.Schools)
                .FirstOrDefaultAsync(sl => sl.Id.ToString() == schoolLevelId);
            if (schoolLevel == null)
                return null;
            var schoolLevelDto = _mapper.Map<SchoolLevelDto>(schoolLevel);
            return schoolLevelDto;
        }
    }
}
