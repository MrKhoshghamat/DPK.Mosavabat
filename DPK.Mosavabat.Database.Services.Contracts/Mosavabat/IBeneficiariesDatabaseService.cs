using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPK.Mosavabat.Database.Services.Contracts.Mosavabat
{
    public interface IBeneficiariesDatabaseService
    {
        Task<GetBeneficiariesLiveTotalDebtDto> GetBeneficiariesLiveTotalDebt(GetBeneficiariesLiveTotalDebtVm vm, CancellationToken cancellationToken=default);
    }
}
