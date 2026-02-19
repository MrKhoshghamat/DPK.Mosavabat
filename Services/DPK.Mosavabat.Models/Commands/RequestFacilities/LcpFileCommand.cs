namespace DPK.Mosavabat.Models.Commands.RequestFacilities;

public class LcpFileCommand(
    string fileLink,
    string name,
    string title,
    long lcpId,
    bool isDeleted)
{
    public string FileLink { get; } = fileLink;
    public string Name { get; } = name;
    public string Title { get; } = title;
    public long LcpId { get; } = lcpId;
    public bool IsDeleted { get; } = isDeleted;
}