using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPK.Mosavabat.Services.Contracts.Mosavabat
{
    public interface IBeneficiaryService
    {
        Task<List<GetBeneficiariesDebtListDto>> GetBeneficiariesDebtList(GetBeneficiariesDebtListVm viewModel, CancellationToken cancellationToken = default);
    }
}
