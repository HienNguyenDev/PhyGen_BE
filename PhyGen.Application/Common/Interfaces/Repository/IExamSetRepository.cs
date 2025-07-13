using PhyGen.Domain.Common;
using PhyGen.Domain.ExamSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Common.Interfaces.Repository
{
    public interface IExamSetRepository : IGenericRepository<ExamSet>
    {
        Task<Page<ExamSet>> GetExamSetsPaginatedAsync(
            int pageNumber,
            int pageSize,
            int? classId = null,
            Guid? createdBy = null,
            string? searchTerm = null);
    }
}
