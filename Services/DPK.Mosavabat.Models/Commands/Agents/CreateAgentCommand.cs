using DPK.Mosavabat.Common.Primitives;
using DPK.Mosavabat.Models.Abstractions.Commands;
using DPK.Mosavabat.Models.EntitiesDto.Agents;

namespace DPK.Mosavabat.Models.Commands.Agents;

public class CreateAgentCommand(
    string branchCode,
    string nationalCode,
    string creationDate,
    string lastName,
    string mobile,
    string name,
    string legalEmail,
    string legalNationalId,
    string legalName)
    : CommandBase<Result<CreateAgentDto>>
{
    public string BranchCode { get; } = branchCode;
    public string NationalCode { get; } = nationalCode;
    public string CreationDate { get; } = creationDate;
    public string LastName { get; } = lastName;
    public string Mobile { get; } = mobile;
    public string Name { get; } = name;
    public string LegalEmail { get; } = legalEmail;
    public string LegalNationalId { get; } = legalNationalId;
    public string LegalName { get; } = legalName;
}