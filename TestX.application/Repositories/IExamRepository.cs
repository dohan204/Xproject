using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestX.application.Dtos;
using TestX.application.Dtos.ExamTestDto;
using TestX.application.Mapping.Exam;
using TestX.domain.Entities.AccountRole;
using TestX.domain.Entities.General;

namespace TestX.application.Repositories
{
    public interface IExamRepository
    {
        Task<List<ExamViewDetailsDto>> GetAllExamDetails();
        Task<int> GetExamCount();
        Task<ExamViewDto> GetExamByName(string Name);
        //Task CreateExamQuestion();
        //Task<>
        Task<int> CreateAsync(ExamCreateDto examCreateDto);
        Task<int> UpdateAsync(ExamUpdateDto examUpdateDto);
        Task<int> CreateExamWithQuestion(ExamCreateDto examCreateDto);
        Task<ExamWithQuestion?> GetDetailsWithQuestion(int id);
        Task<ExamWithQuestion> GetRandomExam();
        Task<int> Delete(int id);
        Task<List<ExamViewDto>> GetExamBySubjectName(int id);
        Task<List<ExamViewDto>> GetBySubject(string name);
        Task<ResultTestDto> HandleDataSubmit(Dictionary<int, string> resultFromFE, int examId);
        Task<int> AddExamFavorites(AddFavoriteExamDto favorite);
        Task<int> RemoveFavorites(int Id);
        Task<List<FavoriteExamViewDto>> GetFavoriteExamsByAccountId(string accountId);
        Task<List<ExamStudentView>> GetExamOfUser(string accountId);
        Task<ResultTestDto> OnSumbitDataFree(Dictionary<int, string> answers, int examId);
    }
}
