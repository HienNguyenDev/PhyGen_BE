using PhyGen.Domain.Common;
using PhyGen.Domain.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Common.Interfaces.Repository
{
    public interface IQuestionRepository : IGenericRepository<Question>
    {
        Task<Page<Question>> GetQuestionsPaginatedAsync(
            int pageNumber,
            int pageSize,
            int? classId = null,
            int? chapterId = null,
            string? searchTerm = null);
    }
}
