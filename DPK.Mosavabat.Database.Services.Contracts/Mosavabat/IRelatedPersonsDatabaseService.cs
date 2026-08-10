using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPK.Mosavabat.Database.Services.Contracts.Mosavabat
{
    public interface IRelatedPersonsDatabaseService
    {
        Task<IsRelatedPersonDto> IsRelatedPersonAsync(IsRelatedPersonVm vm, CancellationToken cancellationToken = default);
        Task<GetTotalDebtOfRelatedPersonsDto> GetTotalDebtOfRelatedPersons(CancellationToken cancellationToken = default);
    }
}
