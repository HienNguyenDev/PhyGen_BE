using PhyGen.Application.Common.Interfaces;
using PhyGen.Application.Common.Interfaces.Repository;
using PhyGen.Domain.Topics;
using PhyGen.Infrastructure.Database;
using PhyGen.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PhyGen.Domain.Common;

namespace PhyGen.Infrastructure.Repositories
{
    public class TopicRepository : GenericRepository<Topic>, ITopicRepository
    {
        public TopicRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Page<Topic>> GetTopicsPaginatedAsync(
            int pageNumber,
            int pageSize,
            Guid? chapterId = null,
            string? searchTerm = null)
        {
            var query = _dbSet.AsQueryable();
            if (chapterId.HasValue)
                query = query.Where(t => t.ChapterId == chapterId);
            if (!string.IsNullOrEmpty(searchTerm))
                query = query.Where(t => t.Keyword.Contains(searchTerm) || t.Synonyms.Contains(searchTerm));
            int count = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return new Page<Topic>(items, count, pageNumber, pageSize);
        }
    }
}
