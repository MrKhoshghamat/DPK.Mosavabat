namespace DPK.Mosavabat.Shared.DataModels.Bsi;

public class AuthDataModel
{
    public string? Access_token { get; set; }
    public int Expires_in { get; set; }
    public string Refresh_token { get; set; }
    public string Token_type { get; set; }

    public DateTime ExpireTime => DateTime.Now.AddSeconds(Expires_in);

    public bool IsExpire()
    {
        return DateTime.Now > ExpireTime;
    }
}