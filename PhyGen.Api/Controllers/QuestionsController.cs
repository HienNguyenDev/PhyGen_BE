using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhyGen.Api.Extensions;
using PhyGen.Application.Common;
using PhyGen.Application.DTOs;
using PhyGen.Application.DTOs.Image;
using PhyGen.Application.DTOs.Questions;
using PhyGen.Application.Services.Questions;
using PhyGen.Domain.Common;
using PhyGen.Domain.Questions;

namespace PhyGen.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class QuestionsController : ControllerBase
{
    private readonly IQuestionServices _questionService;

    public QuestionsController(IQuestionServices questionService)
    {
        _questionService = questionService;
    }

    [HttpPost("process-image")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> ProcessQuestionImage([FromForm] ImageUploadDTO file, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _questionService.ProcessQuestionImageFromPythonAsync(file.File, cancellationToken);

            if (!result.IsSuccess)
            {
                return BadRequest(ApiResult<List<QuestionDTO>>.Failure(result.Message));
            }

            return Ok(ApiResult<List<QuestionDTO>>.Success(result.Data, result.Message));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResult<List<QuestionDTO>>.Failure(ex.Message));
        }
        catch (OperationCanceledException)
        {
            return BadRequest(ApiResult<List<QuestionDTO>>.Failure("The operation was canceled."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResult<List<QuestionDTO>>.Failure($"Internal server error: {ex.Message}"));
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetPaginated([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] Guid? classId = null, [FromQuery] Guid? chapterId = null, [FromQuery] string? searchTerm = null)
    {
        var result = await _questionService.GetPaginatedAsync(pageNumber, pageSize, classId, chapterId, searchTerm);
        if (!result.IsSuccess)
            return BadRequest(ApiResult<Page<Question>>.Failure(result.Message));
        return Ok(ApiResult<Page<Question>>.Success(result.Data, result.Message));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _questionService.GetByIdAsync(id);
        if (!result.IsSuccess)
            return BadRequest(ApiResult<Question>.Failure(result.Message));
        return Ok(ApiResult<Question>.Success(result.Data, result.Message));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Question question, CancellationToken cancellationToken)
    {
        var result = await _questionService.CreateAsync(question, cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(ApiResult<Question>.Failure(result.Message));
        return Ok(ApiResult<Question>.Success(result.Data, result.Message));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] Question question, CancellationToken cancellationToken)
    {
        question.Id = id;
        var result = await _questionService.UpdateAsync(question, cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(ApiResult<Question>.Failure(result.Message));
        return Ok(ApiResult<Question>.Success(result.Data, result.Message));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _questionService.DeleteAsync(id, cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(ApiResult<bool>.Failure(result.Message));
        return Ok(ApiResult<bool>.Success(result.Data, result.Message));
    }
}