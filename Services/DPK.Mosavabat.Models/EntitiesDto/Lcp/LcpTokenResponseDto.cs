using Newtonsoft.Json;

namespace DPK.Mosavabat.Models.EntitiesDto.Lcp;

public class LcpTokenResponseDto
{
    [JsonProperty("accessToken")]
    public string AccessToken { get; set; }

    [JsonProperty("encryptedAccessToken")]
    public string EncryptedAccessToken { get; set; }

    [JsonProperty("expireInSeconds")]
    public int ExpireInSeconds { get; set; }

    [JsonProperty("userId")]
    public int UserId { get; set; }

    [JsonProperty("refreshToken")]
    public string RefreshToken { get; set; }

    [JsonProperty("refreshTokenExpireInSeconds")]
    public string RefreshTokenExpireInSeconds { get; set; }
}