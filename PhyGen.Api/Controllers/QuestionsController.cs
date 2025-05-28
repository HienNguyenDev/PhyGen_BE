using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PhyGen.Application.Services.Questions;
using PhyGen.Domain.Questions;

namespace PhyGen.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuestionsController : ControllerBase
{
    private readonly QuestionService _questionService;

    public QuestionsController(QuestionService questionService)
    {
        _questionService = questionService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Question>>> GetAll()
    {
        var questions = await _questionService.GetQuestionsAsync();
        return Ok(questions);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Question>> GetById(Guid id)
    {
        var question = await _questionService.GetQuestionByIdAsync(id);
        
        if (question is null)
        {
            return NotFound();
        }

        return Ok(question);
    }

    [HttpPost]
    public async Task<ActionResult<Question>> Create([FromBody] Question question)
    {
        try
        {
            var createdQuestion = await _questionService.CreateQuestion(question);
            return CreatedAtAction(nameof(GetById), new { id = createdQuestion.Id }, createdQuestion);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Errors);
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] Question question)
    {
        if (id != question.Id)
        {
            return BadRequest("Id mismatch");
        }

        try
        {
            await _questionService.UpdateQuestionAsync(question);
            return NoContent();
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Errors);
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _questionService.DeleteQuestionAsync(id);
        return NoContent();
    }
}