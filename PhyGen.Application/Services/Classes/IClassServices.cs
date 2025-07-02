using PhyGen.Application.Common;
using PhyGen.Domain.Classes;
using PhyGen.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Services.Classes
{
    public interface IClassServices
    {
        Task<ApiResult<Class>> GetByIdAsync(Guid id);
        Task<ApiResult<Page<Class>>> GetPaginatedAsync(int pageNumber, int pageSize, string? searchTerm = null);
        Task<ApiResult<Class>> CreateAsync(Class classEntity, CancellationToken cancellationToken);
        Task<ApiResult<Class>> UpdateAsync(Class classEntity, CancellationToken cancellationToken);
        Task<ApiResult<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
