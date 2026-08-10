using DPK.Mosavabat.Common.Consts.RelatedPersons;
using DPK.Mosavabat.Shared.DataModels.Mosavabat;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPK.Mosavabat.Shared.Validation.RelatedPersons
{
    public class GetIsRelatedPersonValidator : AbstractValidator<IsRelatedPersonDataModel>
    {
        public GetIsRelatedPersonValidator() 
        {
            RuleFor(r => r.NationalCode)
                .NotNull()
                .WithMessage(IsRelatedPersonConst.validations.NationalCodeMustNotBeNull);
        }
    }
}
