using PhyGen.Application.Common;
using PhyGen.Application.Common.Interfaces;
using PhyGen.Application.Common.Interfaces.Repository;
using PhyGen.Domain.Classes;
using PhyGen.Domain.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhyGen.Application.Services.Classes
{
    public class ClassServices : IClassServices
    {
        private readonly IClassRepository _classRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ClassServices(IClassRepository classRepository, IUnitOfWork unitOfWork)
        {
            _classRepository = classRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResult<Class>> GetByIdAsync(int id)
        {
            var result = await _classRepository.GetByIdAsync(id);
            return result != null 
                ? ApiResult<Class>.Success(result, "Class retrieved successfully") 
                : ApiResult<Class>.Failure("Class not found");
        }

        public async Task<ApiResult<Page<Class>>> GetPaginatedAsync(int pageNumber, int pageSize, string? searchTerm = null)
        {
            var page = await _classRepository.GetClassesPaginatedAsync(pageNumber, pageSize, searchTerm);
            return ApiResult<Page<Class>>.Success(page, "Classes paginated successfully");
        }

        public async Task<ApiResult<Class>> CreateAsync(Class classEntity, CancellationToken cancellationToken)
        {
            await _classRepository.AddAsync(classEntity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return ApiResult<Class>.Success(classEntity, "Class created successfully");
        }

        public async Task<ApiResult<Class>> UpdateAsync(Class classEntity, CancellationToken cancellationToken)
        {
            await _classRepository.UpdateAsync(classEntity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return ApiResult<Class>.Success(classEntity, "Class updated successfully");
        }

        public async Task<ApiResult<bool>> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var classEntity = await _classRepository.GetByIdAsync(id);
            if (classEntity == null)
                return ApiResult<bool>.Failure("Class not found");

            await _classRepository.RemoveAsync(classEntity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return ApiResult<bool>.Success(true, "Class deleted successfully");
        }
    }
}
