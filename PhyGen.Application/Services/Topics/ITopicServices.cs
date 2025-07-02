using PhyGen.Application.Common;
using PhyGen.Domain.Common;
using PhyGen.Domain.Topics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Services.Topics
{
    public interface ITopicServices
    {
        Task<ApiResult<Topic>> GetByIdAsync(Guid id);
        Task<ApiResult<Page<Topic>>> GetPaginatedAsync(int pageNumber, int pageSize, Guid? chapterId = null, string? searchTerm = null);
        Task<ApiResult<Topic>> CreateAsync(Topic topic, CancellationToken cancellationToken);
        Task<ApiResult<Topic>> UpdateAsync(Topic topic, CancellationToken cancellationToken);
        Task<ApiResult<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
