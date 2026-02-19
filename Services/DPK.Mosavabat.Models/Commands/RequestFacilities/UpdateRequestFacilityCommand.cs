using DPK.Mosavabat.Common.Primitives;
using DPK.Mosavabat.Models.Abstractions.Commands;
using DPK.Mosavabat.Models.EntitiesDto.RequestFacilities;
using JetBrains.Annotations;

namespace DPK.Mosavabat.Models.Commands.RequestFacilities;

public class UpdateRequestFacilityCommand(
    string branchCode,
    string caseId,
    string customerNo,
    string accountNumber,
    string requestDate,
    string requestNo,
    string legalName,
    long lcpId,
    bool isDeleted,
    [CanBeNull] string summarizeDifference,
    LcpUpdateHistoryLogsCommand lcpUpdateHistoryLogs,
    UpdateCollateralsCommand createCollaterals,
    UpdateGuarantorsCommand createGuarantors,
    UpdateStatusCommand createStatus,
    UpdateSubjectFacilitiesCommand createSubjectFacilities,
    UpdateLcpFilesCommand createLcpFiles,
    UpdateRequestFacilityStatusesCommand createRequestFacilityStatuses)
    : CommandBase<Result<UpdateRequestFacilityDto>>
{
    public string BranchCode { get; } = branchCode;
    public string CaseId { get; } = caseId;
    public string CustomerNo { get; } = customerNo;
    public string AccountNumber { get; } = accountNumber;
    public string RequestDate { get; } = requestDate;
    public string RequestNo { get; } = requestNo;
    public string LegalName { get; } = legalName;
    public long LcpId { get; } = lcpId;
    public bool IsDeleted { get; } = isDeleted;
    [CanBeNull] public string SummarizeDifference { get; } = summarizeDifference;
    public LcpUpdateHistoryLogsCommand LcpUpdateHistoryLogs { get; } = lcpUpdateHistoryLogs;
    public UpdateCollateralsCommand Collaterals { get; } = createCollaterals;
    public UpdateGuarantorsCommand Guarantors { get; } = createGuarantors;
    public UpdateStatusCommand Status { get; } = createStatus;
    public UpdateSubjectFacilitiesCommand SubjectFacilities { get; } = createSubjectFacilities;
    public UpdateLcpFilesCommand LcpFiles { get; } = createLcpFiles;
    public UpdateRequestFacilityStatusesCommand RequestFacilityStatuses { get; } = createRequestFacilityStatuses;
}