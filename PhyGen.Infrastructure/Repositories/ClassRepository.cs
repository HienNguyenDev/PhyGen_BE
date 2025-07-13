using PhyGen.Application.Common.Interfaces.Repository;
using PhyGen.Domain.Classes;
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
    public class ClassRepository : GenericRepository<Class>, IClassRepository
    {
        public ClassRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Page<Class>> GetClassesPaginatedAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm = null)
        {
            var query = _dbSet.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
                query = query.Where(c => c.ClassName.Contains(searchTerm));
            int count = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return new Page<Class>(items, count, pageNumber, pageSize);
        }
    }
}
