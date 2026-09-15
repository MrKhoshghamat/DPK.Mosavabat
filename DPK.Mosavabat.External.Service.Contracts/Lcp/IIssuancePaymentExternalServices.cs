using DPK.Mosavabat.Shared.DataModels.Lcp;
using DPK.Mosavabat.Shared.EntitiesDto.Lcp;

namespace DPK.Mosavabat.External.Services.Contracts.Lcp
{
    public interface IIssuancePaymentExternalServices
    {
        Task<ProcessIssuancePaymentDto> ProcessIssuancePaymentRequest(ProcessIssuancePaymentDataModel dataModel, CancellationToken cancellationToken = default);
    }
}
