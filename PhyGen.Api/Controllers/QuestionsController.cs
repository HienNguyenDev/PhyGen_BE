using Microsoft.AspNetCore.Mvc;
using PhyGen.Api.Extensions;
using PhyGen.Application.DTOs;
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
    public async Task<IActionResult> ProcessImage(IFormFile file, CancellationToken cancellationToken = default)
    {
        var result = await _questionService.ProcessQuestionImageAsync(file, cancellationToken);

        if (result.IsSuccess)
        {
            return Ok(result); 
        }

        return BadRequest(result); 
    }

}