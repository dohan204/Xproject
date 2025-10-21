using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestX.domain.Entities.General;

namespace TestX.application.Repositories
{
    public interface ISubjectRepository
    {
        Task<List<Subject>> GetAllSubjects();
        Task<Subject> GetSubjectById(int id);
        Task AddAsync(Subject subject);
        Task UpdateAsync(Subject subject, int id);
        Task DeleteAsync(int id);
    }
}
