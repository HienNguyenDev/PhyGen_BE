using PhyGen.Application.Common;
using PhyGen.Application.Common.Interfaces;
using PhyGen.Application.Common.Interfaces.Repository;
using PhyGen.Domain.Chapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Services.Chapters
{
    public class ChapterServices : IChapterServices
    {
        private readonly IChapterRepository _chapterRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ChapterServices(IChapterRepository chapterRepository, IUnitOfWork unitOfWork)
        {
            _chapterRepository = chapterRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResult<IEnumerable<Chapter>>> GetAllChaptersAsync()
        {
            var chapters = await _chapterRepository.GetAllAsync();
            return ApiResult<IEnumerable<Chapter>>.Success(chapters, "Retrieved all chapters successfully");
        }

        public async Task<ApiResult<Chapter>> GetChapterByIdAsync(int id)
        {
            var chapter = await _chapterRepository.GetByIdAsync(id);
            return chapter != null
                ? ApiResult<Chapter>.Success(chapter, "Chapter retrieved successfully")
                : ApiResult<Chapter>.Failure("Chapter not found");
        }

        public async Task<ApiResult<Chapter>> CreateChapterAsync(Chapter chapter, CancellationToken cancellationToken)
        {
            await _chapterRepository.AddAsync(chapter);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return ApiResult<Chapter>.Success(chapter, "Chapter created successfully");
        }

        public async Task<ApiResult<Chapter>> UpdateChapterAsync(Chapter chapter, CancellationToken cancellationToken)
        {
            await _chapterRepository.UpdateAsync(chapter);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return ApiResult<Chapter>.Success(chapter, "Chapter updated successfully");
        }

        public async Task<ApiResult<bool>> DeleteChapterAsync(int id, CancellationToken cancellationToken)
        {
            var chapter = await _chapterRepository.GetByIdAsync(id);
            if (chapter == null)
                return ApiResult<bool>.Failure("Chapter not found");

            await _chapterRepository.RemoveAsync(chapter);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return ApiResult<bool>.Success(true, "Chapter deleted successfully");
        }
    }

}
