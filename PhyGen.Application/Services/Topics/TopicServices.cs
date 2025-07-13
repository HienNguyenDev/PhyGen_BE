using PhyGen.Application.Common;
using PhyGen.Application.Common.Interfaces;
using PhyGen.Application.Common.Interfaces.Repository;
using PhyGen.Domain.Common;
using PhyGen.Domain.Topics;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhyGen.Application.Services.Topics
{
    public class TopicServices : ITopicServices
    {
        private readonly ITopicRepository _topicRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TopicServices(ITopicRepository topicRepository, IUnitOfWork unitOfWork)
        {
            _topicRepository = topicRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResult<Topic>> GetByIdAsync(int id)
        {
            try
            {
                var result = await _topicRepository.GetByIdAsync(id);
                return ApiResult<Topic>.Success(result, "Topic retrieved successfully");
            } catch (Exception ex)
            {
                return ApiResult<Topic>.Failure($"An error occurred while retrieving topic: {ex.Message}");
            }
        }

        public async Task<ApiResult<Page<Topic>>> GetPaginatedAsync(int pageNumber, int pageSize, int? chapterId = null, string? searchTerm = null)
        {
            var page = await _topicRepository.GetTopicsPaginatedAsync(pageNumber, pageSize, chapterId, searchTerm);
            return ApiResult<Page<Topic>>.Success(page, "Topics paginated successfully");
        }

        public async Task<ApiResult<Topic>> CreateAsync(Topic topic, CancellationToken cancellationToken)
        {
            await _topicRepository.AddAsync(topic);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return ApiResult<Topic>.Success(topic, "Topic created successfully");
        }

        public async Task<ApiResult<Topic>> UpdateAsync(Topic topic, CancellationToken cancellationToken)
        {
            await _topicRepository.UpdateAsync(topic);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return ApiResult<Topic>.Success(topic, "Topic updated successfully");
        }

        public async Task<ApiResult<bool>> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var topic = await _topicRepository.GetByIdAsync(id);
            if (topic == null)
                return ApiResult<bool>.Failure("Topic not found");

            await _topicRepository.RemoveAsync(topic);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return ApiResult<bool>.Success(true, "Topic deleted successfully");
        }
    }
}
