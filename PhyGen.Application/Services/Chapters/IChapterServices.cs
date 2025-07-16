using PhyGen.Application.Common;
using PhyGen.Domain.Chapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Services.Chapters
{
    public interface IChapterServices
    {
        Task<ApiResult<IEnumerable<Chapter>>> GetAllChaptersAsync();
        Task<ApiResult<Chapter>> GetChapterByIdAsync(int id);
        Task<ApiResult<Chapter>> CreateChapterAsync(Chapter chapter, CancellationToken cancellationToken);
        Task<ApiResult<Chapter>> UpdateChapterAsync(Chapter chapter, CancellationToken cancellationToken);
        Task<ApiResult<bool>> DeleteChapterAsync(int id, CancellationToken cancellationToken);
    }
}
