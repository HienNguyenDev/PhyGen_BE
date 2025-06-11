using Microsoft.AspNetCore.Mvc;
using PhyGen.Api.Extensions;
using PhyGen.Application.DTOs;
using PhyGen.Application.Services.Users;
using PhyGen.Domain.Common;

namespace PhyGen.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserServices _userService;

        public AuthenticationController(IUserServices userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IResult> RegisterUser(UserDTO user, CancellationToken cancellationToken = default)
        {
            Result result = await _userService.CreateUser(user, cancellationToken);

            return result.MatchOk();
        }

        [HttpPost("login")]
        public async Task<string> LoginUser(LoginDTO user, CancellationToken cancellationToken = default)
        {
            return await _userService.LoginUser(user, cancellationToken);
        }
    }
}
