using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestX.application.Dtos.School;

namespace TestX.application.Repositories
{
    public interface ISchoolService
    {
        Task<List<SchoolLevelDto>> GetSchoolLevelAsync();
        Task<SchoolLevelDto> GetSchoolLevelAsync(string schoolLevelId);

    }
}
