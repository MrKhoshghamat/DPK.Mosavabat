using DPK.Mosavabat.Common.Primitives;
using DPK.Mosavabat.Models.Abstractions.Commands;
using DPK.Mosavabat.Models.EntitiesDto.RequestFacilities;

namespace DPK.Mosavabat.Models.Commands.RequestFacilities;

public class CreateRequestFacilityCommand(
    string branchCode,
    string customerNo,
    string accountNo,
    string requestDate,
    string requestNo,
    string legalName,
    long lcpId,
    bool isDeleted,
    CreateCollateralsCommand collaterals,
    CreateGuarantorsCommand guarantors,
    CreateStatusCommand status,
    CreateSubjectFacilitiesCommand subjectFacilities,
    CreateLcpFilesCommand lcpFiles,
    CreateRequestFacilityStatusesCommand requestFacilityStatuses)
    : CommandBase<Result<CreateRequestFacilityDto>>
{
    public string BranchCode { get; } = branchCode;
    public string CustomerNo { get; } = customerNo;
    public string AccountNo { get; } = accountNo;
    public string RequestDate { get; } = requestDate;
    public string RequestNo { get; } = requestNo;
    public string LegalName { get; } = legalName;
    public long LcpId { get; } = lcpId;
    public bool IsDeleted { get; } = isDeleted;
    public CreateCollateralsCommand Collaterals { get; } = collaterals;
    public CreateGuarantorsCommand Guarantors { get; } = guarantors;
    public CreateStatusCommand Status { get; } = status;
    public CreateSubjectFacilitiesCommand SubjectFacilities { get; } = subjectFacilities;
    public CreateLcpFilesCommand LcpFiles { get; } = lcpFiles;
    public CreateRequestFacilityStatusesCommand RequestFacilityStatuses { get; } = requestFacilityStatuses;
}