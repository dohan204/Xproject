using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestX.application.Dtos.Rating;
using TestX.application.Mapping.Exam;

namespace TestX.application.Repositories
{
    public interface RatingScoreSubject
    {
        Task<List<RatingExamDto>> GetExamAndRating();
        Task<List<AccountWithScoreExamDto>> GetAccountWithScoresAsync();
    }
}
