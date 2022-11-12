using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication.OAuth;
using KanbanAPI.App_Code.Models;

namespace KanbanAPI.Controllers
{
    [Route("{Controller}")]
    public class AuthenticationController : Controller
    {
        private UserManager<User> userManager;
        public AuthenticationController(
            UserManager<User> userManager
            )
        {
            this.userManager = userManager;
        }

        public class SignUpRequest
        {
            public string UserName { get; set; }
            public string Email { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Password { get; set; }
        }

        [HttpPost("Signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequest request)
        {
            var userExists = await userManager.FindByNameAsync(request.UserName);
            if (userExists != null)
                return BadRequest("User already exists!");

            User user = new()
            {
                UserName = request.UserName,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName
            };
            var result = await userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("User created successfully!");
        }


        public class TokenResponse
        {
            public string access_token { get; set; }
            public DateTime access_expires { get; set; }
            public string refresh_token { get; set; }
            public DateTime refresh_expires { get; set; }
        }
        [HttpGet("{username}")]
        public async Task<ActionResult> Get(string username)
        {
            IdentityUser user = null;
            try
            {
              //  user = await userManager.FindByNameAsync(username);
            }
            catch (Exception ex)
            {
                Response.ContentType = "text/plain";
                return StatusCode(StatusCodes.Status401Unauthorized, ex.Message);
            }

            var refreshToken = GenerateRefreshToken();
            TokenResponse response = GenerateTokenResponse(user, refreshToken);
            return Json(response);
        }
        private Guid GenerateRefreshToken()
        {
            using (var provider = new RNGCryptoServiceProvider())
            {
                var bytes = new byte[16];
                provider.GetBytes(bytes);

                return new Guid(bytes);
            }
        }
        private TokenResponse GenerateTokenResponse(IdentityUser user, Guid refresh_token)
        {
            var now = DateTime.UtcNow;
            TokenResponse response = new TokenResponse()
            {
                access_expires = now.AddMinutes(AuthOptions.DurationMins),
                refresh_token = refresh_token.ToString(),
                refresh_expires = now.AddDays(AuthOptions.DurationDays)
            };
            response.access_token = GenerateAccessToken(
                InitClaims(user),
                response.access_expires
            );

            return response;
        }
        private Claim[] InitClaims(IdentityUser user)
        {

            return new Claim[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier,user.Id)
            };
        }
        private string GenerateAccessToken(IEnumerable<Claim> claims, DateTime expiresDate)
        {
            var jwtToken = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                claims: claims,
                notBefore: DateTime.UtcNow,
            expires: expiresDate,
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
            );
            return (new JwtSecurityTokenHandler()).WriteToken(jwtToken);
        }
    }

}