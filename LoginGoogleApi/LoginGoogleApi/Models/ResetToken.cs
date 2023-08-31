using System.ComponentModel.DataAnnotations;

namespace LoginGoogleApi.Models
{
    public class ResetToken
    {
        [Key()]
        public string Email { get; set; } = default!;
        public string Token { get; set; } = default!;
    }
}
