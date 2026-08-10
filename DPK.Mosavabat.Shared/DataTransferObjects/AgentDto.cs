namespace DPK.Mosavabat.Shared.DataTransferObjects;

public record AgentDto
{
    public Guid Id { get; init; }
    public string? Name { get; init; }
    public string? Family { get; init; }
}