using Microsoft.AspNetCore.Mvc;
using SampleLoginAPI.Model;

namespace SampleLoginAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OTPController : ControllerBase
    {
        [HttpPost("verify-sms")]
        public IActionResult VerifySmsOtp([FromBody] OTPVerification model)
        {
            if (OtpStore.UserOtps.TryGetValue(model.EmailOrPhone, out var storedOtp))
            {
                if (storedOtp == model.Otp)
                {
                    OtpStore.UserOtps.Remove(model.EmailOrPhone);
                    return Ok(new { message = "SMS OTP verified successfully." });
                }
                else
                {
                    return BadRequest(new { message = "Invalid SMS OTP." });
                }
            }

            return NotFound(new { message = "No SMS OTP found for this phone number" });
        }

        [HttpPost("verify-email")]
        public IActionResult VerifyEmailOtp([FromBody] OTPVerification model)
        {
            if (OtpStore.UserOtps.TryGetValue(model.EmailOrPhone, out var storedOtp))
            {
                if (storedOtp == model.Otp)
                {
                    OtpStore.UserOtps.Remove(model.EmailOrPhone);
                    return Ok(new { message = "Email OTP verified successfully" });
                }
                else
                {
                    return BadRequest(new { message = "Invalid Email OTP." });
                }
            }

            return NotFound(new { message = "No Email OTP found for this email address." });
        }
    }
}