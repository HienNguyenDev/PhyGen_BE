using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhyGen.Api.Extensions;
using PhyGen.Application.Common;
using PhyGen.Application.DTOs.Authentication;
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
        public async Task<IResult> RegisterUser([FromBody] RegisterUserDTO user, CancellationToken cancellationToken)
        {
            var result = await _userService.CreateUser(user, cancellationToken);

            if (result.IsSuccess && result.Data is not null)
            {
                return Results.Created($"/users/{result.Data.Id}", result); 
            }

            return Results.BadRequest(result); 
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginDTO loginDTO, CancellationToken cancellationToken = default)
        {
            var result = await _userService.LoginUser(loginDTO, cancellationToken);

            if (!result.IsSuccess)
            {
                return Unauthorized(result); 
            }

            return Ok(result); 
        }

        [HttpPost("login-with-google")]
        public async Task<IActionResult> LoginWithGoogle(string uid, CancellationToken cancellationToken = default)
        {
            try
            {
                var jwtToken = await _userService.LoginWithFirebaseUidAsync(uid, cancellationToken);
                return Ok(jwtToken); 
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Login with Google failed: {ex.Message}");
            }
        }

    }
}
