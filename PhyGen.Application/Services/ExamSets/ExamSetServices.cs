using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhyGen.Application.Common;
using PhyGen.Application.Common.Interfaces;
using PhyGen.Application.Common.Interfaces.Repository;
using PhyGen.Domain.Common;
using PhyGen.Domain.ExamSets;

namespace PhyGen.Application.Services.ExamSets
{
    public class ExamSetServices : IExamSetServices
    {
        private readonly IExamSetRepository _examSetRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ExamSetServices(IExamSetRepository examSetRepository, IUnitOfWork unitOfWork)
        {
            _examSetRepository = examSetRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResult<ExamSet>> GetByIdAsync(Guid id)
        {
            var examSet = await _examSetRepository.GetByIdAsync(id);
            if (examSet == null)
                return ApiResult<ExamSet>.Failure("ExamSet not found");
            return ApiResult<ExamSet>.Success(examSet, "ExamSet retrieved successfully");
        }

        public async Task<ApiResult<Page<ExamSet>>> GetPaginatedAsync(int pageNumber, int pageSize, Guid? classId = null, Guid? createdBy = null, string? searchTerm = null)
        {
            var page = await _examSetRepository.GetExamSetsPaginatedAsync(pageNumber, pageSize, classId, createdBy, searchTerm);
            return ApiResult<Page<ExamSet>>.Success(page, "ExamSets paginated successfully");
        }

        public async Task<ApiResult<ExamSet>> CreateAsync(ExamSet examSet, CancellationToken cancellationToken)
        {
            await _examSetRepository.AddAsync(examSet);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return ApiResult<ExamSet>.Success(examSet, "ExamSet created successfully");
        }

        public async Task<ApiResult<ExamSet>> UpdateAsync(ExamSet examSet, CancellationToken cancellationToken)
        {
            var existing = await _examSetRepository.GetByIdAsync(examSet.Id);
            if (existing == null)
                return ApiResult<ExamSet>.Failure("ExamSet not found");
            await _examSetRepository.UpdateAsync(examSet);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return ApiResult<ExamSet>.Success(examSet, "ExamSet updated successfully");
        }

        public async Task<ApiResult<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var examSet = await _examSetRepository.GetByIdAsync(id);
            if (examSet == null)
                return ApiResult<bool>.Failure("ExamSet not found");
            await _examSetRepository.RemoveAsync(examSet);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return ApiResult<bool>.Success(true, "ExamSet deleted successfully");
        }
    }
}
