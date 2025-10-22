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
        public async Task<int> AddAsync(Subject subject)
        {
            await _context.Subjects.AddAsync(subject);
            await _context.SaveChangesAsync();
            return subject.Id;
        }
        public async Task<int> UpdateAsync(Subject subject, int id)
        {
            var subjects = await _context.Subjects.FindAsync(id);
            if(subjects != null)
            {
                subjects.Name = subject.Name;
            }
            await _context.SaveChangesAsync();
            return id;
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
