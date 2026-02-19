namespace DPK.Mosavabat.Models.Commands.RequestFacilities;

public class RequestFacilityStatusCommand(
    string status)
{
    public string Status { get; } = status;
}