using Microsoft.AspNetCore.Mvc;
using SampleLoginAPI.Model;
using Microsoft.Data.SqlClient;
using Dapper;

namespace SampleLoginAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] Login model)
        {
            if (string.IsNullOrEmpty(model.EmailOrPhone) || string.IsNullOrEmpty(model.Password))
            {
                return BadRequest(new { message = "Email/Phone and Password are required" });
            }

            string input = model.EmailOrPhone;
            string password = model.Password;

            using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));

            var query = @"SELECT * FROM Logins WHERE (Email = @input OR Phone = @input) AND Password = @password";
            var user = connection.QueryFirstOrDefault<Login>(query, new { input, password });

            if (user == null)
            {
                return NotFound(new { message = "Invalid Email/Phone or Password" });
            }

            var otp = new Random().Next(100000, 999999).ToString();

            OtpStore.OtpToUser[otp] = input;

            Console.WriteLine($"[Simulated EMAIL/SMS] OTP sent to {input}: {otp}");

            return Ok(new { message = "OTP sent successfully" });
        }

        [HttpPost("Verify-otp")]
        public IActionResult VerifyOtp([FromBody] OTPVerification model)
        {
            if (!OtpStore.OtpToUser.TryGetValue(model.Otp, out var userContact))
            {
                return BadRequest(new { message = "Invalid OTP" });
            }

            OtpStore.OtpToUser.Remove(model.Otp);

            return Ok(new { message = "OTP Verified Successfully" });
        }
    }
}
