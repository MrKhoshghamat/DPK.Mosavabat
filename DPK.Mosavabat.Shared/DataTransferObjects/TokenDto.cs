namespace DPK.Mosavabat.Shared.DataTransferObjects;

public record TokenDto(string AccessToken, string RefreshToken);

public record RepairAccessTokenDto(string RefreshToken);