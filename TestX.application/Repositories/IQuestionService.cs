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
        Task<QuestionViewDto> GetQuestion(int id);
        Task<int> CreateAsync(QuestionCreateDto questionCreateDto);
        Task<int> UpdateAsync(QuestionUpdateDto questionUpdateDto);
        Task DeleteAsync(int id);
        Task<List<QuestionViewDto>> RandomQuestionByLevel(int level, int subjectId, int numberOfQuestion);
        Task<List<QuestionViewDto>> GetPagedQuestionById(int level, int subjectId, int pageSize, int pageNumber);
    }
}
