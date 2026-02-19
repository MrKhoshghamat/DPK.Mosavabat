using DPK.Mosavabat.Common.Primitives;

namespace DPK.Mosavabat.BusinessServices.Features.ApprovalRequests.Errors;

public static class ApprovalRequestErrors
{
    public static readonly Error InvalidBranchCode =
        new("ApprovalRequest.InvalidBranchCode",
            "کد شعبه نامعتبر است.");

    public static readonly Error UsernameNotFound =
        new("ApprovalRequest.UsernameNotFound",
            "کاربر مسئول یافت نشد.");

    public static readonly Error BizagiCaseCreationFailed =
        new("ApprovalRequest.Bizagi.CaseCreationFailed",
            "خطا در ایجاد فرایند در سامانه مصوبات.");
}