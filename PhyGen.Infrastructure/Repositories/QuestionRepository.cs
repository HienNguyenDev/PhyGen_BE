using PhyGen.Application.Common.Interfaces.Repository;
using PhyGen.Domain.Questions;
using PhyGen.Infrastructure.Database;
using PhyGen.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PhyGen.Domain.Common;
using PhyGen.Application.DTOs.ExamSet;

namespace PhyGen.Infrastructure.Repositories
{
    public class QuestionRepository : GenericRepository<Question>, IQuestionRepository
    {
        public QuestionRepository(ApplicationDbContext context) : base(context)
        {
        }
        public IQueryable<Question> GetQueryableWithIncludes()
        {
            return _context.Questions
                .Include(q => q.QuestionOptionSet)
                .Include(q => q.Class)
                .Include(q => q.Chapter)
                .Include(q => q.Topic)
                .AsQueryable();
        }

        public async Task<Page<Question>> GetQuestionsPaginatedAsync(
            int pageNumber,
            int pageSize,
            int? classId = null,
            int? chapterId = null,
            string? searchTerm = null)
        {
            var query = _dbSet.AsQueryable();
            if (classId.HasValue)
                query = query.Where(q => q.ClassId == classId);
            if (chapterId.HasValue)
                query = query.Where(q => q.ChapterId == chapterId);
            if (!string.IsNullOrEmpty(searchTerm))
                query = query.Where(q => q.QuestionContent.Contains(searchTerm));
            int count = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return new Page<Question>(items, count, pageNumber, pageSize);
        }
    }
}
