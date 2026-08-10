using DPK.Mosavabat.Common.Consts.CreditProcesses;
using DPK.Mosavabat.Shared.DataModels.CreditProcesses.PaymentPermission;
using FluentValidation;

namespace DPK.Mosavabat.Shared.Validation.CreditProcesses;

public class EstelamCheckValidator : AbstractValidator<EstelamCheckDataModel>
{
    public EstelamCheckValidator()
    {
        RuleFor(x => x.NationalId)
            .NotNull()
            //.NotEmpty()
            .WithMessage(CreditProcessesConst.Validations.NationalIdMustNotBeNull);
            //.Matches(CreditProcessesConst.Conditions.NumericFormat)
            //.WithMessage(CreditProcessesConst.Validations.NationalIdMustBeNumber);
    }
}