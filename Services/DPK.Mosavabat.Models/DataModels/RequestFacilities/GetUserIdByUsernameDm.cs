namespace DPK.Mosavabat.Models.DataModels.RequestFacilities;

public class GetUserIdByUsernameDm(
    string Username)
{
    public string Username { get; } = Username;
}