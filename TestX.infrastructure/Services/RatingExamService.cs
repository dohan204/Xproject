using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
//using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestX.application.Dtos.Rating;
using TestX.application.Mapping.Exam;
using TestX.application.Repositories;
using TestX.infrastructure.Identity;
using TestX.infrastructure.Migrations;

namespace TestX.infrastructure.Services
{
    public class RatingExamService : RatingScoreSubject
    {
        private readonly IdentityContext _identityContext;
        private readonly ICacheService _cacheService;
        private readonly IMapper mapper;
        public RatingExamService(IdentityContext identityContext, ICacheService cacheService, IMapper mapper)
        {
            _identityContext = identityContext;
            _cacheService = cacheService;
            this.mapper = mapper;
        }
        // get: lấy ra bài thi và số lần thi 
        public async Task<List<RatingExamDto>> GetExamAndRating()
        {
            // lấy ra tất cả thông tin của đề thi 
            var exam = await _identityContext.Exams.Include(e => e.Subject).AsNoTracking().ToListAsync();
            if (exam.Count == 0)
                return new List<RatingExamDto>();
            var dtos = mapper.Map<List<RatingExamDto>>(exam);
            return dtos;
        }
        public async Task<List<AccountWithScoreExamDto>> GetAccountWithScoresAsync()
        {
            var allData = await _identityContext.StudentExams
                .Include(u => u.ApplicationUser)
                .Include(e => e.Exam)
                    .ThenInclude(s => s.Subject)
                .AsNoTracking().ToListAsync();
            if (!allData.Any() || allData.Count == 0)
                return new List<AccountWithScoreExamDto>();

            // map sang dto 
            return mapper.Map<List<AccountWithScoreExamDto>>(allData);
        }
    }
}
