using Microsoft.AspNetCore.Mvc;
using SampleLoginAPI.Model;
using Microsoft.Data.SqlClient;
using Dapper;

namespace SampleLoginAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult Login([FromBody] Login model)
        {
            if (string.IsNullOrEmpty(model.EmailOrPhone) || string.IsNullOrEmpty(model.Password))
                return BadRequest(new { message = "Email/Phone and Password are required" });

            var input = model.EmailOrPhone.Trim();
            var password = model.Password.Trim();

            using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));

            var query = @"SELECT * FROM Logins WHERE (Email = @input OR Phone = @input) AND Password = @password";
            var user = connection.QueryFirstOrDefault<Login>(query, new { input, password });

            if (user == null)
                return NotFound(new { message = "Invalid Email/Phone or Password" });
            var otp = new Random().Next(100000, 999999).ToString();

            OtpStore.UserOtps[input] = otp;

            Console.WriteLine($"Simulated OTP to {input}: {otp}");

            return Ok(new { message = "OTP sent successfully", otp = otp }); 
        }
    }
}

