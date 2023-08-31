using System.Text.Json.Serialization;

namespace LoginGoogleApi.DTO
{
    public class RegisterDto
    {
        [JsonPropertyName("first_name")]
        public string FirsName { get; set; }

        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("password_confirm")]
        public string PasswordConfirm { get; set; }
    }
}
