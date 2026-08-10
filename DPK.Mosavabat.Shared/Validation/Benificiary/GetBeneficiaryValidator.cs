using DPK.Mosavabat.Common.Consts.Beneficiary;
using DPK.Mosavabat.Shared.DataModels.Mosavabat;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPK.Mosavabat.Shared.Validation.Benificiary
{
    public class GetBeneficiaryValidator : AbstractValidator<GetBeneficiariesDebtListDataModel>
    {
        public GetBeneficiaryValidator()
        {
            RuleFor(b => b.NationalCode)
                .NotEmpty()
                .WithMessage(BeneficiaryConst.Validation.NationalCodeMustNotEmpty);

            RuleFor(b => b.NationalCode)
                 .NotNull()
                 .WithMessage(BeneficiaryConst.Validation.NationalCodeMustNotBeNull);
        }
    }
}
