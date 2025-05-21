using Microsoft.AspNetCore.Mvc;
using SampleLoginAPI.Model;

namespace SampleLoginAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OTPController : ControllerBase
    {
        [HttpPost("verify")]
        public IActionResult VerifyOtp([FromBody] OTPVerification model)
        {
            if (!OtpStore.UserOtps.TryGetValue(model.EmailOrPhone, out var storedOtp))
                return NotFound(new { message = "OTP not found for this contact" });

            if (storedOtp != model.Otp)
                return BadRequest(new { message = "Invalid OTP" });

            OtpStore.UserOtps.Remove(model.EmailOrPhone);
            return Ok(new { message = "Login verified successfully" });
        }
    }
}


