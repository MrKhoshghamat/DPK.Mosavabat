namespace DPK.Mosavabat.Models.Commands.RequestFacilities;

public class CreateStatusCommand(
    string code,
    string title)
{
    public string Code { get; } = code;
    public string Title { get; } = title;
}