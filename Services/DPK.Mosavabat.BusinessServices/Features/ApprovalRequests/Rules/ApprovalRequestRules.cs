using DPK.Mosavabat.BusinessServices.Features.ApprovalRequests.Errors;
using DPK.Mosavabat.Common.Primitives;
using DPK.Mosavabat.Models.EntitiesDto.Bizagi;

namespace DPK.Mosavabat.BusinessServices.Features.ApprovalRequests.Rules;

public static class ApprovalRequestRules
{
    public static Result<int> ValidBranchCode(string branchCode)
        => int.TryParse(branchCode, out var value)
            ? Result.Success(value)
            : Result.Failure<int>(ApprovalRequestErrors.InvalidBranchCode);

    public static Result<string> UsernameExists(string username)
        => string.IsNullOrWhiteSpace(username)
            ? Result.Failure<string>(ApprovalRequestErrors.UsernameNotFound)
            : Result.Success(username);

    public static Result<CreateCaseDto> ValidBizagiCase(
        CreateCaseDto caseResult)
        => caseResult.CaseId == 0 || caseResult.RadNumber == "0"
            ? Result.Failure<CreateCaseDto>(
                ApprovalRequestErrors.BizagiCaseCreationFailed)
            : Result.Success(caseResult);
}