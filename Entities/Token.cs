using System.Text.Json.Serialization;
class Token
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }

    [JsonPropertyName("token_type")]
    public string TokenType { get; set; }

    [JsonPropertyName("expires_in")]
    public int Expiration { get; set; }

    [JsonPropertyName("sub")]
    public string Subject { get; set; }
}