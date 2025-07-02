using Microsoft.AspNetCore.Mvc;
using PhyGen.Application.Services.Classes;
using PhyGen.Domain.Classes;
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
    public class ClassesController : ControllerBase
    {
        private readonly IClassServices _classServices;

        public ClassesController(IClassServices classServices)
        {
            _classServices = classServices;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _classServices.GetByIdAsync(id);
            if (!result.IsSuccess)
                return BadRequest(ApiResult<Class>.Failure(result.Message));
            return Ok(ApiResult<Class>.Success(result.Data, result.Message));
        }

        [HttpGet]
        public async Task<IActionResult> GetPaginated([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchTerm = null)
        {
            var result = await _classServices.GetPaginatedAsync(pageNumber, pageSize, searchTerm);
            if (!result.IsSuccess)
                return BadRequest(ApiResult<Page<Class>>.Failure(result.Message));
            return Ok(ApiResult<Page<Class>>.Success(result.Data, result.Message));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Class classEntity, CancellationToken cancellationToken)
        {
            var result = await _classServices.CreateAsync(classEntity, cancellationToken);
            if (!result.IsSuccess)
                return BadRequest(ApiResult<Class>.Failure(result.Message));
            return Ok(ApiResult<Class>.Success(result.Data, result.Message));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Class classEntity, CancellationToken cancellationToken)
        {
            classEntity.Id = id;
            var result = await _classServices.UpdateAsync(classEntity, cancellationToken);
            if (!result.IsSuccess)
                return BadRequest(ApiResult<Class>.Failure(result.Message));
            return Ok(ApiResult<Class>.Success(result.Data, result.Message));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var result = await _classServices.DeleteAsync(id, cancellationToken);
            if (!result.IsSuccess)
                return BadRequest(ApiResult<bool>.Failure(result.Message));
            return Ok(ApiResult<bool>.Success(result.Data, result.Message));
        }
    }
}
