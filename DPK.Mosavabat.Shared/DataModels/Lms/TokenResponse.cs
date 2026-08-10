using System.Security.AccessControl;

namespace DPK.Mosavabat.Shared.DataModels.Lms;

public sealed record TokenResponse(int ExpireInSeconds, int UserId, string RefreshToken,
    string RefreshTokenExpireInSeconds, string EncryptedAccessToken, string AccessToken, bool UnAuthorized = false)
{
    public static TokenResponse IsUnAuthorized => new(0, 0, string.Empty, string.Empty, string.Empty, string.Empty, true);
    public static TokenResponse IsAuthorized(TokenResponse response) => response;
};