using Google.Apis.Auth;
using LoginGoogleApi.Data;
using LoginGoogleApi.DTO;
using LoginGoogleApi.Models;
using LoginGoogleApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace LoginGoogleApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class AuthController : Controller
    {
        private IConfiguration configuration;
        private ApplicationDbContext db;

        public AuthController(IConfiguration configuration, ApplicationDbContext db)
        {
            this.configuration = configuration;
            this.db = db;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterDto registerDto)
        {
            if(registerDto.Password != registerDto.PasswordConfirm)
            {
                return Unauthorized("Passord devem ser iguais");
            }

            User user = new User
            {
                Email = registerDto.Email,
                Password = HasService.HashPassword(registerDto.Password),
                FirstName = registerDto.FirsName,
                LastName = registerDto.LastName
            };

            db.Users.Add(user);
            db.SaveChanges();

            return Ok(user);
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDto loginDto)
        {
            User? user =  db.Users.Where(x => x.Email == loginDto.Email).FirstOrDefault();

            if(user == null)
            {
                return Unauthorized("Credenciais inválidas");
            }

            if(HasService.HashPassword(loginDto.Password) != user.Password)
            {
                return Unauthorized("Senha inválida");
            }

            string accessToken = TokenService.CreateAccessToken(user.Id, configuration.GetSection("JWT:AccessKey").Value);
            string refreshToken = TokenService.CreateRefreshToken(user.Id, configuration.GetSection("JWT:RefreshKey").Value);

            CookieOptions cookieOptions = new();
            cookieOptions.HttpOnly = true;
            Response.Cookies.Append("refresh_token", refreshToken, cookieOptions);

            UserToken token = new()
            {
                UserId = user.Id,
                Token = refreshToken,
                ExpiredAt = DateTime.Now.AddDays(7)
            };

            db.UserTokens.Add(token);
            db.SaveChanges();

            return Ok(new
            {
                token = accessToken
            });
        }

        [HttpGet("user")]
        public new IActionResult User()
        {
            string authorizationHeader = Request.Headers["Authorization"];

            if(authorizationHeader is null || authorizationHeader.Length <= 8)
            {
                return Unauthorized("Não autenticado!");
            }

            string accessToken = authorizationHeader[7..];

            var id = TokenService.DecodeToken(accessToken, out bool hasTokenExpired);

            if (hasTokenExpired)
            {
                return Unauthorized("Não autenticado!");
            }

            User? user = db.Users.Where(user => user.Id == id).FirstOrDefault();

            if (user is null)
            {
                return Unauthorized("Não autenticado!");
            }

            return Ok(user);
        }

        [HttpPost("refresh")]
        public IActionResult Refresh()
        {
            if (Request.Cookies["refresh_token"] is null)
            {
                return Unauthorized("Não autenticado!");
            };

            string? refreshToken = Request.Cookies["refresh_token"];

            int id = TokenService.DecodeToken(refreshToken, out bool hasTokenExpired);

            if(!db.UserTokens.Where(u => u.UserId == id && u.Token == refreshToken && u.ExpiredAt > DateTime.Now).Any())
            {
                return Unauthorized("Não autenticado!");
            };

            if (hasTokenExpired)
            {
                return Unauthorized("Não autenticado!");
            };

            string accessToken = TokenService.CreateAccessToken(id, configuration.GetSection("JWT:AccessKey").Value);

            return Ok(new { 
                token = accessToken
            });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {

            string? refreshToken = Request.Cookies["refresh_token"];

            if (refreshToken is null)
            {
                return Ok("Pronto, realizado o logout!");
            }

            db.UserTokens.Remove(db.UserTokens.Where(u => u.Token == refreshToken).First());
            db.SaveChanges();

            Response.Cookies.Delete("refresh_token");

            return Ok("Logout realizado com sucesso");
        }

        [HttpPost("google-auth")]
        public async Task<IActionResult> GoogleAuth(GoogleAuthDto dto)
        {
            var googleUser = await GoogleJsonWebSignature.ValidateAsync(dto.Token);

            if(googleUser is null)
            {
                return Unauthorized("Não autenticado!");
            }

            User? user = db.Users.Where(u => u.Email == googleUser.Email).FirstOrDefault();
            
            if(user is null)
            {
                user = new()
                {
                    FirstName = googleUser.GivenName,
                    LastName = googleUser.FamilyName,
                    Email = googleUser.Email,
                    Password = dto.Token
                };

                db.Users.Add(user);
                db.SaveChanges();

                user.Id = db.Users.Where(u => u.Email == user.Email).FirstOrDefault()!.Id;
            }

            string accessToken = TokenService.CreateAccessToken(user.Id, configuration.GetSection("JWT:AccessKey").Value);
            string refreshToken = TokenService.CreateRefreshToken(user.Id, configuration.GetSection("JWT:RefreshKey").Value);

            CookieOptions cookieOptions = new();
            cookieOptions.HttpOnly = true;
            Response.Cookies.Append("refresh_token", refreshToken, cookieOptions);

            UserToken token = new()
            {
                UserId = user.Id,
                Token = refreshToken,
                ExpiredAt = DateTime.Now.AddDays(7)
            };

            db.UserTokens.Add(token);
            db.SaveChanges();

            return Ok(new
            {
                token = accessToken
            });
        }
    }
}
