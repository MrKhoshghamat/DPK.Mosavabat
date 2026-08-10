using DPK.Mosavabat.Common.Consts.CreditProcesses;
using DPK.Mosavabat.Shared.DataModels.Lms;
using FluentValidation;

namespace DPK.Mosavabat.Shared.Validation.CreditProcesses;

public class PaymentPermissionIssuanceControlValidator : AbstractValidator<PaymentPermissionIssuanceControlDataModel>
{
    public PaymentPermissionIssuanceControlValidator()
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

        RuleFor(x => x.ContractAmount)
            .NotNull()
            //.NotEmpty()
            .WithMessage(CreditProcessesConst.Validations.ContractAmountMustNotBeNull);
        //.MaximumLength(CreditProcessesConst.Conditions.ContractAmountMaxLength)
        //.WithMessage(CreditProcessesConst.Validations.ContractAmountMaxLength)
        //.Must(CreditProcessesConst.ValidationHelper.CheckIfValueIsValid)
        //.WithMessage(CreditProcessesConst.Validations.ContractAmountIsNotValid);

        RuleFor(x => x.CustomerSharePrice)
            .NotNull()
            .WithMessage(CreditProcessesConst.Validations.CustomerSharePriceMustNotBeNull);
        //.GreaterThanOrEqualTo(CreditProcessesConst.Conditions.CustomerSharePriceMinValue)
        //.WithMessage(CreditProcessesConst.Validations.CustomerSharePriceCanNotBeNegative)
        //.Must(CreditProcessesConst.ValidationHelper.CheckIfValueIsValid)
        //.WithMessage(CreditProcessesConst.Validations.CustomerSharePriceIsNotValid);

        RuleFor(x => x.SubContractType)
            .NotNull()
            //.NotEmpty()
            .WithMessage(CreditProcessesConst.Validations.SubContractTypeMustNotBeNull);
            //.GreaterThanOrEqualTo(CreditProcessesConst.Conditions.SubContractTypeMinValue)
            //.WithMessage(CreditProcessesConst.Validations.SubContractTypeCanNotBeNegative)
            //.Must(CreditProcessesConst.ValidationHelper.CheckIfValueIsValid)
            //.WithMessage(CreditProcessesConst.Validations.SubContractTypeIsNotValid);
    }
}