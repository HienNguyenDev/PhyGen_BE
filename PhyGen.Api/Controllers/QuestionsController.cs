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
public class QuestionsController : ControllerBase
{
    private readonly IQuestionService _questionService;

    public QuestionsController(IQuestionService questionService)
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

}