using DPK.Mosavabat.Common.Consts.CreditProcesses;
using DPK.Mosavabat.Shared.DataModels.CreditProcesses.PaymentPermission;
using FluentValidation;

namespace DPK.Mosavabat.Shared.Validation.CreditProcesses;

public class CalculateCustomerDebtBalanceValidator : AbstractValidator<CalculationCustomerDebtBalanceDataModel>
{
    public CalculateCustomerDebtBalanceValidator()
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