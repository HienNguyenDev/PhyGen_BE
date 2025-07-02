using PhyGen.Domain.Common;
using PhyGen.Domain.Topics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Common.Interfaces.Repository
{
    public interface ITopicRepository : IGenericRepository<Topic>
    {
        Task<Page<Topic>> GetTopicsPaginatedAsync(
            int pageNumber,
            int pageSize,
            Guid? chapterId = null,
            string? searchTerm = null);
    }
}
