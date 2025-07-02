using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhyGen.Application.Common;
using PhyGen.Domain.Common;
using PhyGen.Domain.ExamSets;

namespace PhyGen.Application.Services.ExamSets
{
    public interface IExamSetServices
    {
        Task<ApiResult<ExamSet>> GetByIdAsync(Guid id);
        Task<ApiResult<Page<ExamSet>>> GetPaginatedAsync(int pageNumber, int pageSize, Guid? classId = null, Guid? createdBy = null, string? searchTerm = null);
        Task<ApiResult<ExamSet>> CreateAsync(ExamSet examSet, CancellationToken cancellationToken);
        Task<ApiResult<ExamSet>> UpdateAsync(ExamSet examSet, CancellationToken cancellationToken);
        Task<ApiResult<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
