using DPK.Mosavabat.Common.Consts.CreditProcesses;
using DPK.Mosavabat.Shared.ViewModels.Abrisham.Loan;
using FluentValidation;

namespace DPK.Mosavabat.Shared.Validation.CreditProcesses
{
    public class GetLoanInstallmentCollectionTransactionsValidator : AbstractValidator<GetLoanInstallmentCollectionTransactionsVm>
    {
        public GetLoanInstallmentCollectionTransactionsValidator()
        {
            // شماره تسهیلات
            RuleFor(x => x.LoanId)
                .NotNull()
                //.NotEmpty()
                .WithMessage(CreditProcessesConst.Validations.LoanIdMustNotBeNull);
                //.Matches(CreditProcessesConst.Conditions.NumericFormat)
                //.WithMessage(CreditProcessesConst.Validations.LoanIdMustBeNumber);
        }
    }
}
