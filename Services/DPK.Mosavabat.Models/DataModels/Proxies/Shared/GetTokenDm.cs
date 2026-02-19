namespace DPK.Mosavabat.Models.DataModels.Proxies.Shared;

public class GetTokenDm(
    string username,
    string password,
    string clientId)
{
    public string Username { get; } = username;
    public string Password { get; } = password;
    public string ClientId { get; } = clientId;
}