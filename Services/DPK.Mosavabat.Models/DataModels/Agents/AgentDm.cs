using JetBrains.Annotations;

namespace DPK.Mosavabat.Models.DataModels.Agents;

public class AgentDm(
    [CanBeNull] string branchCode,
    [CanBeNull] string nationalCode,
    [CanBeNull] string lastName,
    [CanBeNull] string mobile,
    [CanBeNull] string name,
    [CanBeNull] string legalNationalId,
    [CanBeNull] string legalName)
{
    [CanBeNull] public string BranchCode { get; } = branchCode;
    [CanBeNull] public string NationalCode { get; } = nationalCode;
    [CanBeNull] public string LastName { get; } = lastName;
    [CanBeNull] public string Mobile { get; } = mobile;
    [CanBeNull] public string Name { get; } = name;
    [CanBeNull] public string LegalNationalId { get; } = legalNationalId;
    [CanBeNull] public string LegalName { get; } = legalName;
}