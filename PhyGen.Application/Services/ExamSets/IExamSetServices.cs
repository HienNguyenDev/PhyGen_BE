using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhyGen.Application.Common;
using PhyGen.Application.DTOs.ExamSet;
using PhyGen.Domain.Common;
using PhyGen.Domain.ExamSets;
using PhyGen.Domain.Questions;

namespace PhyGen.Application.Services.ExamSets
{
    public interface IExamSetServices
    {
        Task<ApiResult<List<ExamResult>>> GenerateExamFromDropDownAsync(int quantity, int Chapter, int Topic, int Class);
        Task<ApiResult<List<ExamResult>>> GenerateExamFromPromptAsync(string prompt);
        Task<(byte[] FileBytes, string FileName)> GenerateExamPdfAsync(Guid examSetId);
        Task<ApiResult<ExamSet>> GetByIdAsync(Guid id);
        Task<ApiResult<Page<ExamSet>>> GetPaginatedAsync(int pageNumber, int pageSize, int? classId = null, Guid? createdBy = null, string? searchTerm = null);
        Task<ApiResult<ExamSet>> CreateAsync(ExamSet examSet, CancellationToken cancellationToken);
        Task<ApiResult<ExamSet>> UpdateAsync(ExamSet examSet, CancellationToken cancellationToken);
        Task<ApiResult<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken);
        //Task<List<ExamSet>> GetExamSetsByCurrentUserAsync();
    }
}
