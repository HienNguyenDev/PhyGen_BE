using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhyGen.Application.Common;
using PhyGen.Application.DTOs.FirebaseNotification;
using PhyGen.Application.Services.Firebase;

namespace PhyGen.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotifyController : ControllerBase
    {
        private readonly IFirebaseServices _firebaseService;

        public NotifyController(IFirebaseServices firebaseService)
        {
            _firebaseService = firebaseService;
        }

        [HttpPost("register-token")]
        public async Task<IActionResult> RegisterToken([FromBody] FcmTokenDTO dto)
        {
            try
            {
                var success = await _firebaseService.SaveTokenToFirebaseAsync(dto.UserId, dto.FcmToken);

                if (success)
                {
                    return Ok(new { message = "Token saved to Firebase successfully." });
                }
                else return StatusCode(500, "Failed to save token.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResult<bool>.Failure($"Internal server error: {ex.Message}"));
            }

        }
    }
}