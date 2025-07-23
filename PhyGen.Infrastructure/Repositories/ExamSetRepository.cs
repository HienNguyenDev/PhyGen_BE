using PhyGen.Application.Common.Interfaces.Repository;
using PhyGen.Domain.ExamSets;
using PhyGen.Infrastructure.Database;
using PhyGen.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PhyGen.Domain.Common;
using Microsoft.AspNetCore.Http.HttpResults;

namespace PhyGen.Infrastructure.Repositories
{
    public class ExamSetRepository : GenericRepository<ExamSet>, IExamSetRepository
    {
        public ExamSetRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<ExamSet> GetByIdWithQuestionsAsync(Guid examSetId)
        {
            return await _context.ExamSets
                .Include(es => es.ExamSetQuestions)
                .ThenInclude(esq => esq.Question)
                .ThenInclude(q => q.QuestionOptionSet)
                .FirstOrDefaultAsync(es => es.Id == examSetId);
        }

        public async Task<IEnumerable<ExamSet>> GetExamSetsByCreatorAsync(Guid createdBy)
        {
            return await _context.ExamSets
                .Where(e => e.CreatedBy == createdBy)
                .Include(e => e.ExamSetQuestions)
                    .ThenInclude(esq => esq.Question)
                    .ThenInclude(q => q.QuestionOptionSet)
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<ExamSet>> GetExamSetsCreatedByAsync(Guid userId)
        {
            return await _context.ExamSets
            .Include(es => es.Class)
            .Include(es => es.ExamSetQuestions)
            .Where(es => es.CreatedBy == userId)
            .ToListAsync();
        }

        public async Task<Page<ExamSet>> GetExamSetsPaginatedAsync(
            int pageNumber,
            int pageSize,
            int? classId = null,
            Guid? createdBy = null,
            string? searchTerm = null)
        {
            var query = _dbSet.AsQueryable();
            if (classId.HasValue)
                query = query.Where(e => e.ClassId == classId);
            if (createdBy.HasValue)
                query = query.Where(e => e.CreatedBy == createdBy);
            if (!string.IsNullOrEmpty(searchTerm))
                query = query.Where(e => e.Title.Contains(searchTerm) || e.Description.Contains(searchTerm));
            int count = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return new Page<ExamSet>(items, count, pageNumber, pageSize);
        }
    }
}
