using DPK.Mosavabat.Common.Consts.CreditProcesses;
using DPK.Mosavabat.Shared.DataModels.Lms;
using FluentValidation;

namespace DPK.Mosavabat.Shared.Validation.CreditProcesses;

public class CheckAndUpdateC13InPaymentPermissionValidator : AbstractValidator<CheckAndUpdateC13InPaymentPermissionDataModel>
{
    public CheckAndUpdateC13InPaymentPermissionValidator()
    {
        RuleFor(x => x.CustomerNo)
            .NotNull()
            //.NotEmpty()
            .WithMessage(CreditProcessesConst.Validations.CustomerNoMustNotBeNull);
            //.Must(CreditProcessesConst.ValidationHelper.CustomerNoMustGreaterThanValidator)
            //.WithMessage(CreditProcessesConst.Validations.CustomerNoMustGreaterThan)
            //.MaximumLength(CreditProcessesConst.Conditions.CustomerNoMaxLength)
            //.WithMessage(CreditProcessesConst.Validations.CustomerNoMaxLength)
            //.Matches(CreditProcessesConst.Conditions.NumericFormat)
            //.WithMessage(CreditProcessesConst.Validations.CustomerNoMustBeNumber);

        RuleFor(x => x.BranchCode)
            .NotNull()
            //.NotEmpty()
            .WithMessage(CreditProcessesConst.Validations.BranchCodeMustNotBeNull);
        //.MaximumLength(CreditProcessesConst.Conditions.BranchCodeMaxLength)
        //.WithMessage(CreditProcessesConst.Validations.BranchCodeMaxLength);
        //.Must(CreditProcessesConst.ValidationHelper.CheckIfValueIsValid)
        //.WithMessage(CreditProcessesConst.Validations.BranchCodeIsNotValid);

        RuleFor(x => x.ContractType)
            .NotNull()
            //.NotEmpty()
            .WithMessage(CreditProcessesConst.Validations.ContractTypeMustNotBeNull);
        //.MaximumLength(CreditProcessesConst.Conditions.ContractTypeMaxLength)
        //.WithMessage(CreditProcessesConst.Validations.ContractTypeMaxLength)
        //.Must(CreditProcessesConst.ValidationHelper.CheckIfValueIsValid)
        //.WithMessage(CreditProcessesConst.Validations.ContractTypeIsNotValid);

        RuleFor(x => x.SubContractType)
            .NotNull()
            //.NotEmpty()
            .WithMessage(CreditProcessesConst.Validations.SubContractTypeMustNotBeNull);
            //.MaximumLength(CreditProcessesConst.Conditions.SubContractTypeMaxLength)
            //.WithMessage(CreditProcessesConst.Validations.SubContractTypeMaxLength)
            //.Must(CreditProcessesConst.ValidationHelper.CheckIfValueIsValid)
            //.WithMessage(CreditProcessesConst.Validations.SubContractTypeIsNotValid);

        RuleFor(x => x.ContractPrepaidAmountsSum)
            .NotNull()
            //.NotEmpty()
            .WithMessage(CreditProcessesConst.Validations.ContractPrepaidAmountsSumMustNotBeNull);
        //.MaximumLength(CreditProcessesConst.Conditions.ContractPrepaidAmountsSumMaxLength)
        //.WithMessage(CreditProcessesConst.Validations.ContractPrepaidAmountsSumMaxLength)
        //.Must(CreditProcessesConst.ValidationHelper.CheckIfValueIsValid)
        //.WithMessage(CreditProcessesConst.Validations.ContractPrepaidAmountsSumIsNotValid);

        RuleFor(x => x.C13)
            .NotNull()
            //.NotEmpty()
            .WithMessage(CreditProcessesConst.Validations.C13MustNotBeNull);
            //.GreaterThanOrEqualTo(CreditProcessesConst.Conditions.C13MinValue)
            //.LessThanOrEqualTo(CreditProcessesConst.Conditions.C13MaxValue)
            //.WithMessage(CreditProcessesConst.Validations.C13ExactlyLength)
            //.Must(CreditProcessesConst.ValidationHelper.CheckIfValueIsValid)
            //.WithMessage(CreditProcessesConst.Validations.C13IsNotValid);
    }
}