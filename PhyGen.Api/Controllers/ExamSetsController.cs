using Microsoft.AspNetCore.Mvc;
using PhyGen.Application.Services.ExamSets;
using PhyGen.Domain.ExamSets;
using PhyGen.Application.Common;
using PhyGen.Domain.Common;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using PhyGen.Application.DTOs.ExamSet;

namespace PhyGen.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExamSetsController : ControllerBase
    {
        private readonly IExamSetServices _examSetServices;

        public ExamSetsController(IExamSetServices examSetServices)
        {
            _examSetServices = examSetServices;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _examSetServices.GetByIdAsync(id);
            if (!result.IsSuccess)
                return BadRequest(ApiResult<ExamSet>.Failure(result.Message));
            return Ok(ApiResult<ExamSet>.Success(result.Data, result.Message));
        }

        [HttpGet]
        public async Task<IActionResult> GetPaginated([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] int? classId = null, [FromQuery] Guid? createdBy = null, [FromQuery] string? searchTerm = null)
        {
            var result = await _examSetServices.GetPaginatedAsync(pageNumber, pageSize, classId, createdBy, searchTerm);
            if (!result.IsSuccess)
                return BadRequest(ApiResult<Page<ExamSet>>.Failure(result.Message));
            return Ok(ApiResult<Page<ExamSet>>.Success(result.Data, result.Message));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ExamSet examSet, CancellationToken cancellationToken)
        {
            var result = await _examSetServices.CreateAsync(examSet, cancellationToken);
            if (!result.IsSuccess)
                return BadRequest(ApiResult<ExamSet>.Failure(result.Message));
            return Ok(ApiResult<ExamSet>.Success(result.Data, result.Message));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ExamSet examSet, CancellationToken cancellationToken)
        {
            examSet.Id = id;
            var result = await _examSetServices.UpdateAsync(examSet, cancellationToken);
            if (!result.IsSuccess)
                return BadRequest(ApiResult<ExamSet>.Failure(result.Message));
            return Ok(ApiResult<ExamSet>.Success(result.Data, result.Message));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var result = await _examSetServices.DeleteAsync(id, cancellationToken);
            if (!result.IsSuccess)
                return BadRequest(ApiResult<bool>.Failure(result.Message));
            return Ok(ApiResult<bool>.Success(result.Data, result.Message));
        }

        [HttpPost("generate-exam-from-prompt")]
        public async Task<IActionResult> GenerateExamFromPrompt([FromBody] PromptRequest request)
        {
            var result = await _examSetServices.GenerateExamFromPromptAsync(request.Prompt);
            if (!result.IsSuccess)
                return BadRequest(ApiResult<List<ExamResult>>.Failure(result.Message));

            return Ok(ApiResult<List<ExamResult>>.Success(result.Data, result.Message));
        }

        [HttpPost("generate-exam-from-dropdown")]
        public async Task<IActionResult> GenerateExamFromDropDown([FromBody] DropDownRequest request)
        {
            var result = await _examSetServices.GenerateExamFromDropDownAsync(request.Quantity, request.Chapter, request.Topic, request.Class);
            if (!result.IsSuccess)
                return BadRequest(ApiResult<List<ExamResult>>.Failure(result.Message));

            return Ok(ApiResult<List<ExamResult>>.Success(result.Data, result.Message));
        }
    }
}
