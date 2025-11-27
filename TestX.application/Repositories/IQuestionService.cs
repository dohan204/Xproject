using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestX.application.Dtos.Question;

namespace TestX.application.Repositories
{
    public interface IQuestionService
    {
        Task<List<QuestionViewDto>> AllQuestion();
        Task<int> GetAllCountQuestion();
        Task<QuestionViewDto> GetQuestion(int id);
        Task<int> CreateAsync(QuestionCreateDto questionCreateDto);
        Task<int> UpdateAsync(QuestionUpdateDto questionUpdateDto);
        Task DeleteAsync(int id);
        Task<List<QuestionViewDto>> RandomQuestionByLevel(int level, int subjectId, int numberOfQuestion);
        Task<PagedResult<QuestionViewDto>> GetPagedQuestionById(int level, int subjectId, int pageSize = 10, int pageNumber = 1);
        Task<int> Delete(int id);
    }
}
