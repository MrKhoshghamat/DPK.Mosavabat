using DPK.Mosavabat.Common.Consts.CreditProcesses;
using DPK.Mosavabat.Shared.DataModels.Lms;
using FluentValidation;
using FluentValidation.Results;
using System;

namespace DPK.Mosavabat.Shared.Validation.CreditProcesses;

public class InsertPaymentPermissionValidator : AbstractValidator<InsertPaymentPermissionDataModel>
{

    public InsertPaymentPermissionValidator()
    {
        RuleFor(x => x.MosavabeNo)
            .NotNull()
            //.NotEmpty()
            .WithMessage(CreditProcessesConst.Validations.MosavabeNoMustNotBeNull);
        //.MaximumLength(CreditProcessesConst.Conditions.MosavabeNoMaxLength)
        //.WithMessage(CreditProcessesConst.Validations.MosavabeNoMaxLength)
        //.MinimumLength(CreditProcessesConst.Conditions.MosavabeNoMinLength)
        //.WithMessage(CreditProcessesConst.Validations.MosavabeNoMinLength)
        //.Matches(CreditProcessesConst.Conditions.NumericFormat)
        //.WithMessage(CreditProcessesConst.Validations.MosavabeNoMustBeNumber);

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

        RuleFor(x => x.ContractType)
            .NotNull()
            //.NotEmpty()
            .WithMessage(CreditProcessesConst.Validations.ContractTypeMustNotBeNull);
        //.GreaterThanOrEqualTo(CreditProcessesConst.Conditions.ContractTypeMinValue)
        //.WithMessage(CreditProcessesConst.Validations.ContractTypeCanNotBeNegative)
        //.Must(CreditProcessesConst.ValidationHelper.CheckIfValueIsValid)
        //.WithMessage(CreditProcessesConst.Validations.ContractTypeIsNotValid);

        RuleFor(x => x.SubContractType)
            .NotNull()
            //.NotEmpty()
            .WithMessage(CreditProcessesConst.Validations.SubContractTypeMustNotBeNull);
        //.GreaterThanOrEqualTo(CreditProcessesConst.Conditions.SubContractTypeMinValue)
        //.WithMessage(CreditProcessesConst.Validations.SubContractTypeCanNotBeNegative)
        //.Must(CreditProcessesConst.ValidationHelper.CheckIfValueIsValid)
        //.WithMessage(CreditProcessesConst.Validations.SubContractTypeIsNotValid);


        RuleFor(x => x.ContractAmount)
            .NotNull()
            //.NotEmpty()
            .WithMessage(CreditProcessesConst.Validations.ContractAmountMustNotBeNull);
            //.GreaterThanOrEqualTo(CreditProcessesConst.Conditions.ContractAmountMinValue)
            //.WithMessage(CreditProcessesConst.Validations.ContractAmountCanNotBeNegative)
            //.Must(CreditProcessesConst.ValidationHelper.CheckIfValueIsValid)
            //.WithMessage(CreditProcessesConst.Validations.ContractAmountIsNotValid);


        //RuleFor(x => x.PrepaidAmount)
        //    .GreaterThanOrEqualTo(CreditProcessesConst.Conditions.PrepaidAmountMinValue)
        //    .WithMessage(CreditProcessesConst.Validations.PrepaidAmountCanNotBeNegative)
        //    .Must(CreditProcessesConst.ValidationHelper.CheckIfValueIsValid)
        //    .WithMessage(CreditProcessesConst.Validations.PrepaidAmountIsNotValid);
    }
}