using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
//using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestX.application.Repositories;
using TestX.domain.Entities.General;
using TestX.infrastructure.Identity;
using TestX.application.Dtos.Subject;

namespace TestX.infrastructure.Services
{
    public class SubjectService : ISubjectRepository
    {
        private readonly IdentityContext _context;
        public SubjectService(IdentityContext _context)
        {
            this._context = _context;
        }
        public async Task<Subject> GetSubjectById(int id)
        {
            return await _context.Subjects.FirstOrDefaultAsync(e => e.Id == id);
        }
        public async Task<List<Subject>> GetAllSubjects()
        {
            return await _context.Subjects.AsNoTracking().ToListAsync();
        }
        public async Task<int> AddAsync(Add ad)
        {
            var sub = new Subject
            {
                Name = ad.Name,
                Code = ad.Code
            };
            _context.Subjects.Add(sub);
            await _context.SaveChangesAsync();
            return 1;
        }
        public async Task<int> UpdateAsync(Update up, int id)
        {
            var subject = _context.Subjects.FirstOrDefault(e => e.Id == id);
            if (subject == null)
                return 0;
            var updatedSubject = new Subject
            {
                Name = up.Name,
                Code = up.Code
            };
            _context.Subjects.Update(updatedSubject);
            await _context.SaveChangesAsync();
            return 1;
        }
        public async Task<int> DeleteAsync(int id)
        {
            var subject = await GetSubjectById(id);
            if(subject != null)
                _context.Subjects.Remove(subject);
            await _context.SaveChangesAsync();
            return id;
        }
    }
}
