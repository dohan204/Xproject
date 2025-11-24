using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestX.application.Dtos.Subject;
using TestX.domain.Entities.General;

namespace TestX.application.Repositories
{
    public interface ISubjectRepository
    {
        Task<List<Subject>> GetAllSubjects();
        Task<Subject> GetSubjectById(int id);
        Task<int> AddAsync(Add ad);
        Task<int> UpdateAsync(Update subject, int id);
        Task<int> DeleteAsync(int id);
    }
}
