using Microsoft.AspNetCore.Mvc;
using PhyGen.Api.Extensions;
using PhyGen.Application.DTOs;
using PhyGen.Application.Services.Users;
using PhyGen.Domain.Common;

namespace PhyGen.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserServices _userService;

        public AuthController(IUserServices userService)
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
        public async Task<TokenResponse> LoginUser(LoginDTO user, CancellationToken cancellationToken = default)
        {
            return await _userService.LoginUser(user, cancellationToken);
        }
    }
}
