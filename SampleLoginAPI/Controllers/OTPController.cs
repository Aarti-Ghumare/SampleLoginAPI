using Microsoft.AspNetCore.Mvc;
using SampleLoginAPI.Model;

namespace SampleLoginAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OTPController : ControllerBase
    {
        private IActionResult Verify(string key, string providedOtp, string type)
        {
            if (OtpStore.UserOtps.TryGetValue(key, out var storedOtp))
            {
                if (storedOtp == providedOtp)
                {
                    OtpStore.UserOtps.Remove(key); 
                    return Ok(new { message = $"{type} OTP verified successfully." });
                }
                else
                {
                    return BadRequest(new { message = $"Invalid {type} OTP." });
                }
            }

                return NotFound(new { message = $"No OTP found for this {type}." });
        }

        [HttpPost("verify-sms")]
        public IActionResult VerifySmsOtp([FromBody] OTPVerification model)
        {
            return Verify(model.EmailOrPhone, model.Otp, "SMS");
        }

        [HttpPost("verify-email")]
        public IActionResult VerifyEmailOtp([FromBody] OTPVerification model)
        {
            return Verify(model.EmailOrPhone, model.Otp, "Email");
        }
    }
}
