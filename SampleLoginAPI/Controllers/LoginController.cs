using Microsoft.AspNetCore.Mvc;
using System.Data;
using Dapper;
using SampleLoginAPI.Model;
using Microsoft.Data.SqlClient;

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
            if (string.IsNullOrWhiteSpace(model.EmailOrPhone) || string.IsNullOrWhiteSpace(model.Password))
            {
                return BadRequest(new { message = "Email/Phone and Password are required." });
            }

            string input = model.EmailOrPhone.Trim();
            string password = model.Password.Trim();

            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                string query = @"
                    SELECT * FROM Logins 
                    WHERE Password = @Password 
                    AND (Email COLLATE SQL_Latin1_General_CP1_CI_AS = @Input 
                         OR Phone = @Input)";

                var user = db.QueryFirstOrDefault<Login>(query, new
                {
                    Input = input,
                    Password = password
                });

                if (user != null)
                {
                    
                    var otp = new Random().Next(100000, 999999).ToString();

                    OtpStore.UserOtps[input] = otp;

                    Console.WriteLine($"Simulated OTP sent to {input}: {otp}");

                    return Ok(new
                    {
                        message = "Login successful. OTP sent to your registered contact.",
                        otp = otp 
                    });
                }
                else
                {
                    return NotFound(new { message = "Invalid Email/Phone or Password." });
                }
            }
        }
    }
}
