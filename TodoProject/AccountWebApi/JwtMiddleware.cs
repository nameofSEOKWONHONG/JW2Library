using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AccountService;
using eXtensionSharp;
using JWLibrary;
using JWLibrary.ServiceExecutor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using TodoService.Data;

namespace AccountWebApi {
    /// <summary>
    ///     mvc : https://jasonwatmore.com/post/2019/10/11/aspnet-core-3-jwt-authentication-tutorial-with-example-api
    ///     api :
    ///     https://jasonwatmore.com/post/2020/07/21/aspnet-core-3-create-and-validate-jwt-tokens-use-custom-jwt-middleware
    /// </summary>
    public class JwtMiddleware {
        public static readonly Lazy<JWTSettings> JwtSettings =
            new(() => new JWTSettings());

        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next) {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IGetAccountSvc getAccountSvc) {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null)
                await AttachAccountToContext(context, token, getAccountSvc);

            await _next(context);
        }

        private async Task AttachAccountToContext(HttpContext context, string token, IGetAccountSvc getAccountSvc) {
            try {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(JwtSettings.Value.Secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out var validatedToken);

                var jwtToken = (JwtSecurityToken) validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == "USER").Value;

                // attach account to context on successful jwt validation
                using var executor = new ServiceExecutorManager<IGetAccountSvc>(getAccountSvc);
                executor.SetRequest(o => o.Request =  userId)
                    .OnExecuted(o => {
                        context.Items["USER"] = o.Result;
                        return true;
                    });
                await Task.Delay(1);
            }
            catch {
                // do nothing if jwt validation fails
                // account is not attached to context so request won't have access to secure routes
            }
        }
    }

    public class JwtTokenService {
        public string GenerateJwtToken(string userId) {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(JwtMiddleware.JwtSettings.Value.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(new[] {new Claim("USER", userId)}),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string ValidateJwtToken(string token) {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(JwtMiddleware.JwtSettings.Value.Secret);
            try {
                tokenHandler.ValidateToken(token, new TokenValidationParameters {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out var validatedToken);

                var jwtToken = (JwtSecurityToken) validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == "USER").Value;

                // return account id from JWT token if validation successful
                return userId;
            }
            catch {
                // return null if validation fails
                return null;
            }
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter {
        public void OnAuthorization(AuthorizationFilterContext context) {
            var user = (USER) context.HttpContext.Items["USER"];
            if (user.xIsNull()) // not logged in
                context.Result = new JsonResult(new {message = "Unauthorized"})
                    {StatusCode = StatusCodes.Status401Unauthorized};
        }
    }
}