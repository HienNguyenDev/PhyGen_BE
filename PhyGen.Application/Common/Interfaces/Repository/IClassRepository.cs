using PhyGen.Domain.Classes;
using PhyGen.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Common.Interfaces.Repository
{
    public interface IClassRepository : IGenericRepository<Class>
    {
        Task<Page<Class>> GetClassesPaginatedAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm = null);
    }
}
