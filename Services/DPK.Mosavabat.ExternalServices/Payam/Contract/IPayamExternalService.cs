using DPK.Foundation.Dependency;
using DPK.Mosavabat.Models.DataModels.Payam;
using DPK.Mosavabat.Models.EntitiesDto.Payam;

namespace DPK.Mosavabat.ExternalServices.Payam.Contract;

public interface IPayamExternalService
    : ITransientDependency
{
    Task<CheckInquiryDto> CheckInquiry(
        CheckInquiryDm dm,
        CancellationToken cancellationToken = default);
}