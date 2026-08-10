using DPK.Mosavabat.Common.Consts.CreditProcesses;
using DPK.Mosavabat.Shared.ViewModels.CreditProcesses.Condition;
using FluentValidation;

namespace DPK.Mosavabat.Shared.Validation.CreditProcesses;

public class ConditionValidator : AbstractValidator<ConditionVm>
{
    public ConditionValidator()
    {
        RuleFor(x => x.Title)
            .NotNull()
            //.NotEmpty()
            .WithMessage(CreditProcessesConst.Validations.ConditionTitleMustNotBeNull);
    }
}