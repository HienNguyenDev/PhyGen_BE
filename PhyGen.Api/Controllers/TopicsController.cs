using Microsoft.AspNetCore.Mvc;
using PhyGen.Application.Services.Topics;
using PhyGen.Domain.Topics;
using PhyGen.Application.Common;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PhyGen.Domain.Common;

namespace PhyGen.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TopicsController : ControllerBase
    {
        private readonly ITopicServices _topicServices;

        public TopicsController(ITopicServices topicServices)
        {
            _topicServices = topicServices;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _topicServices.GetByIdAsync(id);
            if (!result.IsSuccess)
                return BadRequest(ApiResult<Topic>.Failure(result.Message));
            return Ok(ApiResult<Topic>.Success(result.Data, result.Message));
        }

        [HttpGet]
        public async Task<IActionResult> GetPaginated([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] Guid? chapterId = null, [FromQuery] string? searchTerm = null)
        {
            var result = await _topicServices.GetPaginatedAsync(pageNumber, pageSize, chapterId, searchTerm);
            if (!result.IsSuccess)
                return BadRequest(ApiResult<Page<Topic>>.Failure(result.Message));
            return Ok(ApiResult<Page<Topic>>.Success(result.Data, result.Message));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Topic topic, CancellationToken cancellationToken)
        {
            var result = await _topicServices.CreateAsync(topic, cancellationToken);
            if (!result.IsSuccess)
                return BadRequest(ApiResult<Topic>.Failure(result.Message));
            return Ok(ApiResult<Topic>.Success(result.Data, result.Message));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Topic topic, CancellationToken cancellationToken)
        {
            topic.Id = id;
            var result = await _topicServices.UpdateAsync(topic, cancellationToken);
            if (!result.IsSuccess)
                return BadRequest(ApiResult<Topic>.Failure(result.Message));
            return Ok(ApiResult<Topic>.Success(result.Data, result.Message));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var result = await _topicServices.DeleteAsync(id, cancellationToken);
            if (!result.IsSuccess)
                return BadRequest(ApiResult<bool>.Failure(result.Message));
            return Ok(ApiResult<bool>.Success(result.Data, result.Message));
        }
    }
}
