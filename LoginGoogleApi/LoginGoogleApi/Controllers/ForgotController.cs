using LoginGoogleApi.Data;
using LoginGoogleApi.DTO;
using LoginGoogleApi.Models;
using LoginGoogleApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace LoginGoogleApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class ForgotController : Controller
    {
        private ApplicationDbContext db;

        public ForgotController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [HttpPost("forgot")]
        public IActionResult Forgot(ForgotDto forgotDto)
        {
            ResetToken resetToken = new()
            {
                Email = forgotDto.Email,
                Token = Guid.NewGuid().ToString(),
            };

            db.ResetTokens.Add(resetToken);
            db.SaveChanges();

            MailService.SendPasswordResetMailAsync(resetToken);

            return Ok(new
            {
                message = "Email enviado com link para resetar seu password!"
            });
        }

        [HttpPost("reset")]
        public IActionResult Reset(ResetDto resetDto)
        {
            if (resetDto.Password != resetDto.PasswordConfirm)
            {
                return Unauthorized("Passord devem ser iguais");
            }

            ResetToken? resetToken = db.ResetTokens.Where(t => t.Token == resetDto.Token).FirstOrDefault();

            if (resetToken is null)
            {
                return BadRequest("Link Invalido!");
            }

            User? user = db.Users.Where(u => u.Email == resetToken.Email).FirstOrDefault();

            if(user is null)
            {
                return BadRequest("Usuário não encontrado!");
            }

            db.Users.Where(u => u.Email == user.Email).FirstOrDefault().Password = HasService.HashPassword(resetDto.Password);
            db.SaveChanges();

            return Ok(new
            {
                message = "Senha alterada com sucesso!"
            });
        }
    }
}
