using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication.OAuth;
using KanbanAPI.App_Code.Models;
using KanbanAPI.App_Code;
using System.Text;

namespace KanbanAPI.Controllers
{

    public class TokenController : Controller
    {
        private IUserRepository userRepository;
        private ITokenRepository tokenRepository;
        public TokenController(
            IUserRepository userRepository,
            ITokenRepository tokenRepository
            )
        {
            this.userRepository = userRepository;
            this.tokenRepository = tokenRepository;
        }

        public class SignUpRequest
        {
            public string UserName { get; set; }
            public string Email { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Password { get; set; }
        }

        [HttpPost("token/SignUp")]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequest request)
        {
            var userExists = await userRepository.GetByUserNameAsync(request.UserName);
            if (userExists != null)
                return BadRequest("User already exists!");

            User user = new()
            {
                UserName = request.UserName,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Password = PasswordHandler.GetHashString(request.Password, "N_&*(_N&(*0nbn-8v654v956N&)*(_mnytugfu")
            };
            userRepository.AddPart(user);
            await userRepository.SaveChangesAsync();

            return Ok();
        }

        public class SignInRequest
        {
            public string UserName { get; set; }
            public string Password { get; set; }
        }
        public class TokenResponse
        {
            public string access_token { get; set; }
            public DateTime access_expires { get; set; }
            public string refresh_token { get; set; }
            public DateTime refresh_expires { get; set; }
        }
        [HttpPost("Token")]
        public async Task<ActionResult> Post([FromBody] SignInRequest request)
        {
            User user = null;
            try
            {
                user = await userRepository.GetByUserNameAsync(request.UserName);
                string passHash = PasswordHandler.GetHashString(request.Password, "N_&*(_N&(*0nbn-8v654v956N&)*(_mnytugfu");
                if (user == null || user.Password != passHash)
                {
                    Response.ContentType = "application/json";
                    return StatusCode(StatusCodes.Status401Unauthorized, "User not found or password is incorect");
                }

            }
            catch (Exception ex)
            {
                Response.ContentType = "application/json";
                return StatusCode(StatusCodes.Status401Unauthorized, ex.Message);
            }

            var refreshToken = GenerateRefreshToken();
            TokenResponse response = GenerateTokenResponse(user, refreshToken);
            Token token = new Token()
            {
                UserID = user.ID,
                TokenID = refreshToken,
                Expires = response.refresh_expires
            };
            tokenRepository.AddPart(token);
            await tokenRepository.SaveChangesAsync();
            return Json(response);
        }
        public class PutRequest
        {
            public string access_token { get; set; }
            public string refresh_token { get; set; }
        }
        [HttpPut("Token")]
        public async Task<ActionResult> Put([FromBody] PutRequest request)
        {
            try
            {
                JwtSecurityToken jwtToken;
                ClaimsPrincipal principal = ValidateToken(request.access_token, out jwtToken);
                int userID = ClaimsHelper.GetUserIdentifier(principal);

                Token token = tokenRepository.Get(userID, new Guid(request.refresh_token));

                if (token == null)
                {
                    tokenRepository.DeleteByUserID(userID);
                    await tokenRepository.SaveChangesAsync();
                    throw new SecurityTokenException("Invalid token. All your active logins will be deactivated for security reasons.");
                }
                else
                {
                    DateTime refreshTokenExpires = token.Expires;
                    tokenRepository.DeletePart(token);
                    await tokenRepository.SaveChangesAsync();

                    if (DateTime.UtcNow > refreshTokenExpires)
                    {
                        throw new SecurityTokenException("Your authentication session is over. Sign in again.");
                    }
                }

                User user = userRepository.GetByID(userID);
                if (user == null)
                {
                    throw new SecurityTokenException("User found");
                }

                var refreshToken = GenerateRefreshToken();

                TokenResponse response = GenerateTokenResponse(user, refreshToken);

                token = new Token()
                {
                    UserID = user.ID,
                    TokenID = refreshToken,
                    Expires = response.refresh_expires
                };
                tokenRepository.AddPart(token);
                await tokenRepository.SaveChangesAsync();
                Response.ContentType = "application/json";
                return Json(response);
            }
            catch (SecurityTokenException ex)
            {
                Response.ContentType = "application/json";
                return StatusCode((int)StatusCodes.Status403Forbidden, ex.Message);
            }
            catch (Exception ex)
            {
                Response.ContentType = "application/json";
                return BadRequest(ex.Message);
            }
        }
        public class DeleteRequest
        {
            public string access_token { get; set; }
            public string refresh_token { get; set; }
        }
        [HttpDelete("Token")]
        public async Task<ActionResult> Delete([FromBody] DeleteRequest request)
        {
            try
            {
                JwtSecurityToken jwtToken;
                ClaimsPrincipal principal = ValidateToken(request.access_token, out jwtToken);
                int userID = ClaimsHelper.GetUserIdentifier(principal);

                Token token = tokenRepository.Get(userID, new Guid(request.refresh_token));

                if (token != null)
                {
                    tokenRepository.DeletePart(token);
                    await tokenRepository.SaveChangesAsync();
                }

                Response.ContentType = "application/json";
                return Ok();
            }
            catch (SecurityTokenException ex)
            {
                Response.ContentType = "application/json";
                return StatusCode((int)StatusCodes.Status403Forbidden, ex.Message);
            }
            catch (Exception ex)
            {
                Response.ContentType = "application/json";
                return BadRequest(ex.Message);
            }
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
        private TokenResponse GenerateTokenResponse(User user, Guid refresh_token)
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
        private Claim[] InitClaims(User user)
        {

            return new Claim[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier,user.ID.ToString())
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
        private ClaimsPrincipal ValidateToken(string token, out JwtSecurityToken jwtSecurityToken)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
    }

}