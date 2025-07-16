using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhyGen.Application.Services.Chapters;
using PhyGen.Domain.Chapters;

namespace PhyGen.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ChapterController : ControllerBase
    {
        private readonly IChapterServices _chapterServices;
        public ChapterController(IChapterServices chapterServices)
        {
            _chapterServices = chapterServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllChapters()
        {
            var result = await _chapterServices.GetAllChaptersAsync();
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetChapterById(int id)
        {
            var result = await _chapterServices.GetChapterByIdAsync(id);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }
        [HttpPost]
        public async Task<IActionResult> CreateChapter([FromBody] Chapter chapter, CancellationToken cancellationToken)
        {
            if (chapter == null)
                return BadRequest("Chapter cannot be null");

            var result = await _chapterServices.CreateChapterAsync(chapter, cancellationToken);
            return CreatedAtAction(nameof(GetChapterById), new { id = result.Data.Id }, result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateChapter(int id, [FromBody] Chapter chapter, CancellationToken cancellationToken)
        {
            if (chapter == null || chapter.Id != id)
                return BadRequest("Chapter data is invalid");

            var result = await _chapterServices.UpdateChapterAsync(chapter, cancellationToken);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChapter(int id, CancellationToken cancellationToken)
        {
            var result = await _chapterServices.DeleteChapterAsync(id, cancellationToken);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }
    }
}
