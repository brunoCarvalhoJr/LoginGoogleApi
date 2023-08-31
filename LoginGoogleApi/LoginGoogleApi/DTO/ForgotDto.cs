using System.Text.Json.Serialization;

namespace LoginGoogleApi.DTO
{
    public class ForgotDto
    {

        [JsonPropertyName("email")]
        public string Email { get; set; }
    }
}
