using DPK.Mosavabat.Common.Primitives;
using DPK.Mosavabat.Models.Abstractions.Commands;
using DPK.Mosavabat.Models.EntitiesDto.RequestFacilities;

namespace DPK.Mosavabat.Models.Commands.RequestFacilities;

/// <summary>
/// ورودی هندلر سرویس ویرایش شماره پیگیری درخواست تسهیلات
/// </summary>
/// <param name="idApprovalRequest"></param>
/// <param name="radNumber"></param>
public class UpdateRequestFacilityRadNumberCommand(
    int idApprovalRequest,
    string radNumber)
    : CommandBase<Result<UpdateRequestFacilityRadNumberDto>>
{
    public int IdApprovalRequest { get; } = idApprovalRequest;
    public string RadNumber { get; } = radNumber;
}