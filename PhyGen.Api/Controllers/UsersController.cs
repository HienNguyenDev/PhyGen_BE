using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhyGen.Api.Extensions;
using PhyGen.Application.DTOs;
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
        
        public UsersController(IUserServices userService)
        {
            _userService = userService;
        }

        [Authorize]
        [HttpGet("get-users-paginated")]
        public async Task<Page<User>> GetUsersPagnition(int pageNumber, int pageSize)
        {
            var result = await _userService.GetUsersPaginatedAsync(pageNumber, pageSize);
            return result;
        }


        [HttpGet("by-email/{email}")]
        public async Task<IResult> GetUserByEmail(string email)
        {
            var result = await _userService.GetByEmailAsync(email);
            return result.MatchOk();
        }

        [HttpGet("by-username/{username}")]
        public async Task<IResult> GetUserByUsername(string username)
        {
            var result = await _userService.GetByUsernameAsync(username);
            return result.MatchOk();
        }

        [HttpGet("by-role/{role}")]
        public async Task<IResult> GetUsersByRole(UserRole role)
        {
            var result = await _userService.GetUsersByRoleAsync(role);
            return result.MatchOk();
        }

        [HttpPut("Update")]
        public async Task<IResult> UpdateUser(User user, CancellationToken cancellationToken = default)
        {
            var result = await _userService.UpdateUser(user, cancellationToken);
            return result.MatchOk();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("InActive/{userId}")]
        public async Task<IResult> InActiveUser(Guid userId, CancellationToken cancellationToken = default)
        {
            var result = await _userService.InActiveUser(userId, cancellationToken);
            return result.MatchOk();
        }

    }
}
