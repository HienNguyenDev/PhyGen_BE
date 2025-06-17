using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhyGen.Api.Extensions;
using PhyGen.Application.Abstractions.Authentication;
using PhyGen.Application.Common;
using PhyGen.Application.DTOs;
using PhyGen.Application.DTOs.Users;
using PhyGen.Application.Services.Questions;
using PhyGen.Application.Services.Users;
using PhyGen.Domain.Common;
using PhyGen.Domain.Users;

namespace PhyGen.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserServices _userService;
        private readonly IUserContext _userContext;

        public UsersController(IUserServices userService, IUserContext userContext)
        {
            _userService = userService;
            _userContext = userContext;
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser(CancellationToken cancellationToken)
        {
            var result = await _userService.GetCurrentUser(cancellationToken);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpPut("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserDTO updateUserDto, CancellationToken cancellationToken)
        {
            var message = await _userService.UpdateUser(id, updateUserDto, cancellationToken);
            return Ok(new { message });
        }

        [HttpPatch("{id:guid}/inactivate")]
        [Authorize]
        public async Task<IActionResult> InactivateUser(Guid id, CancellationToken cancellationToken)
        {
            var message = await _userService.InActiveUser(id, cancellationToken);
            return Ok(new { message });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUsersPaginated(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] UserRole? roleFilter = null,
            [FromQuery] UserStatus? statusFilter = null,
            [FromQuery] string? searchTerm = null)
        {
            var pageResult = await _userService.GetUsersPaginatedAsync(pageNumber, pageSize, roleFilter, statusFilter, searchTerm);
            return Ok(pageResult);
        }

        [HttpGet("get-user-by-email-or-username")]
        [Authorize]
        public async Task<IActionResult> GetUserByEmailOrUsername([FromQuery] string identifier)
        {
            var result = await _userService.GetByEmailOrUsernameAsync(identifier);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpGet("get-user-by-role")]
        [Authorize]
        public async Task<IActionResult> GetUsersByRole([FromQuery] UserRole role)
        {
            try
            {
                var users = await _userService.GetUsersByRoleAsync(role);
                return Ok(ApiResult<List<User>>.Success(users, $"Users with role '{role}' retrieved successfully."));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResult<List<User>>.Failure(ex.Message));
            }
        }
    }
}
