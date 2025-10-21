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
using Microsoft.AspNetCore.Mvc.Filters;
using AutoMapper.QueryableExtensions;
using TestX.application.Mapping;
using TestX.domain.Entities.General;

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
        public async Task<SchoolDto> GetSchoolById(int id)
        {
            var school = await _context.School.Include(e => e.SchoolLevel).FirstOrDefaultAsync(sc => sc.Id == id);
            var dtos = _mapper.Map<SchoolDto>(school);
            //var school = await _context.School.ProjectTo<SchoolDto>(_mapper.ConfigurationProvider)
            //    .FirstOrDefaultAsync(sc => sc.SchoolLevelId == id);
            return dtos;
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
        public async Task AddAsync(CreateSchoolDto schoolDto)
        {
            if(schoolDto == null)
                throw new ArgumentNullException(nameof(schoolDto));

            var school = _mapper.Map<School>(schoolDto);
            school.CreatedAt = DateTime.Now;
            await _context.School.AddAsync(school);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(UpdateSchoolDto schoolDto, int id)
        {
            var school = await _context.School.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
            if (school == null)
                throw new ArgumentNullException($"trường với id: {id} khong tìm thấy");

            //if(school.Id == school.Id)
            //{
            //    school.Name = schoolDto.Name;
            //    school.Address = schoolDto.Address;
            //    school.SchoolLevelId = schoolDto.SchoolLevelId;
            //    school.ModifiedAt = DateTime.Now;
            //    school.SchoolCode = schoolDto.Code;
            //    school.PhoneNumber = schoolDto.PhoneNumber;
            //    school.Email = schoolDto.Email;
            //}
            schoolDto.ModifiedAt = DateTime.Now;
            var updateSchool = _mapper.Map<UpdateSchoolDto, School>(schoolDto, school);
            _context.School.Update(updateSchool);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var school = await _context.School.AsNoTracking().FirstOrDefaultAsync(sc => sc.Id == id);
            if (school == null)
                throw new ArgumentNullException("không tìm thấy trường");
            _context.School.Remove(school);
            await _context.SaveChangesAsync();
        }
    }
}
