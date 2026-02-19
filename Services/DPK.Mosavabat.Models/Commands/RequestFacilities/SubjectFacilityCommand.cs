using JetBrains.Annotations;

namespace DPK.Mosavabat.Models.Commands.RequestFacilities;

public class SubjectFacilityCommand(
    string amount,
    string contractPeriod,
    string contractPeriodType,
    string description,
    string subjectContract,
    [CanBeNull] string subContractType,
    string facilityGroup,
    RequestDocumentsCommand requestDocuments,
    long lcpId,
    bool isDeleted)
{
    public string Amount { get; } = amount;
    public string ContractPeriod { get; } = contractPeriod;
    public string ContractPeriodType { get; } = contractPeriodType;
    public string Description { get; } = description;
    public string SubjectContract { get; } = subjectContract;
    [CanBeNull] public string SubContractType { get; } = subContractType;
    public string FacilityGroup { get; } = facilityGroup;
    public RequestDocumentsCommand RequestDocuments { get; } = requestDocuments;
    public long LcpId { get; } = lcpId;
    public bool IsDeleted { get; } = isDeleted;
}