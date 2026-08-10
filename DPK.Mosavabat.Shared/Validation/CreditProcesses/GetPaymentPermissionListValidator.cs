using DPK.Mosavabat.Common.Consts.CreditProcesses;
using DPK.Mosavabat.Shared.DataModels.Lms;
using FluentValidation;

namespace DPK.Mosavabat.Shared.Validation.CreditProcesses;

public class GetPaymentPermissionListValidator : AbstractValidator<GetPaymentPermissionListDataModel>
{
    public GetPaymentPermissionListValidator()
    {
        RuleFor(x => x.MosavabeNumber)
            .NotNull()
            //.NotEmpty()
            .WithMessage(CreditProcessesConst.Validations.MosavabeNoMustNotBeNull);
            //.MaximumLength(CreditProcessesConst.Conditions.MosavabeNoMaxLength)
            //.WithMessage(CreditProcessesConst.Validations.MosavabeNoMaxLength)
            //.MinimumLength(CreditProcessesConst.Conditions.MosavabeNoMinLength)
            //.WithMessage(CreditProcessesConst.Validations.MosavabeNoMinLength)
            //.Matches(CreditProcessesConst.Conditions.NumericFormat)
            //.WithMessage(CreditProcessesConst.Validations.MosavabeNoMustBeNumber);
    }
}