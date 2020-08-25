using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace figma.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class LoginController : Controller
    {
        private IConfiguration _config;

        public LoginController(IConfiguration config)
        {
            _config = config;
        }

        public class UserModel
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string EmailAddress { get; set; }
            public DateTime DateOfJoing { get; set; }
        }

        public IActionResult Login2()
        {

            return View();
        }



        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login2([Bind] UserModel login)
        {
            //  var user = AuthenticateUser(login);
            var user = AuthenticateUser(login);
            user = new UserModel { Username = "Jignesh Trivedi", EmailAddress = "test.btest@gmail.com", DateOfJoing = new DateTime(2010, 08, 02) };
            //var user = new UserModel { Username = "Jignesh Trivedi", EmailAddress = "test.btest@gmail.com", DateOfJoing = new DateTime(2010, 08, 02) };
            Console.WriteLine(user);
            if (user != null)
            {
                var tokenString = GenerateJSONWebToken(user);
                return Ok(new { token = tokenString });
            }

            return Ok(new { h="1"});
        }

        private string GenerateJSONWebToken(UserModel userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.Username),
                new Claim(JwtRegisteredClaimNames.Email, userInfo.EmailAddress),
                new Claim("DateOfJoing", userInfo.DateOfJoing.ToString("yyyy-MM-dd")),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private UserModel AuthenticateUser(UserModel login)
        {
            UserModel user = null;

            //Validate the User Credentials
            //Demo Purpose, I have Passed HardCoded User Information
            if (login.Username == "Jignesh")
            {
                user = new UserModel { Username = "Jignesh Trivedi", EmailAddress = "test.btest@gmail.com", DateOfJoing = new DateTime(2010, 08, 02) };
            }
            return user;
        }
    }
}
