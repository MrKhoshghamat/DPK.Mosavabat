using DPK.Mosavabat.Models.Abstractions.Commands;
using DPK.Mosavabat.Models.EntitiesDto.Lcp;

namespace DPK.Mosavabat.Models.Commands.Lcp;

/// <summary>
/// ورودی هندلر سرویس بروزرسانی شماره پیگیری درخواست تسهیلات
/// </summary>
/// <param name="caseId"></param>
/// <param name="radNumber"></param>
/// <param name="caseIdApprovalRequest"></param>
public class UpdateRequestFacilityRadNumberCommand(
    int caseId,
    int radNumber,
    int caseIdApprovalRequest) 
    : CommandBase<UpdateLcpRequestFacilityRadNumberDto>
{
    public int CaseId { get; } = caseId;
    public int RadNumber { get; } = radNumber;
    public int CaseIdApprovalRequest { get; } = caseIdApprovalRequest;
}