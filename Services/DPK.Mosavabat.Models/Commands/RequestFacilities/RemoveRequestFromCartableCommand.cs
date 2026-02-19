using DPK.Mosavabat.Common.Primitives;
using DPK.Mosavabat.Models.Abstractions.Commands;
using DPK.Mosavabat.Models.EntitiesDto.RequestFacilities;

namespace DPK.Mosavabat.Models.Commands.RequestFacilities;

/// <summary>
/// ورودی هندلر سرویس حذف درخواست از کارتابل
/// </summary>
/// <param name="caseId"></param>
public class RemoveRequestFromCartableCommand(
    int caseId)
    : CommandBase<Result<RemoveRequestFromCartableDto>>
{
    public int CaseId { get; } = caseId;
}