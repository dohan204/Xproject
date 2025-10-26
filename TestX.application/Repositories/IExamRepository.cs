using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestX.application.Dtos.ExamTestDto;
using TestX.domain.Entities.General;

namespace TestX.application.Repositories
{
    public interface IExamRepository
    {
        Task<List<ExamViewDetailsDto>> GetAllExamDetails();
        Task<ExamViewDto> GetExamByName(string Name);
        //Task CreateExamQuestion();
        //Task<>
        Task<int> CreateAsync(ExamCreateDto examCreateDto);
        Task<int> UpdateAsync(ExamUpdateDto examUpdateDto);
        Task<int> CreateExamWithQuestion(ExamCreateDto examCreateDto);
        Task<ExamWithQuestion?> GetDetailsWithQuestion(int id);
    }
}
