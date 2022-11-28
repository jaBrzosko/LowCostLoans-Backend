using System.Text.Json.Serialization;

namespace Services.Data.Auth0;


public class Auth0Profile
{
    [JsonPropertyName("sub")]
    public string Sub { get; set; } = null!;
    [JsonPropertyName("nickname")]
    public string Nickname { get; set; } = null!;
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;
    [JsonPropertyName("picture")]
    public string Picture { get; set; } = null!;
    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; set; }
    [JsonPropertyName("email")]
    public string Mail { get; set; } = null!;
    [JsonPropertyName("email_verified")]
    public bool MailVerified { get; set; }
}