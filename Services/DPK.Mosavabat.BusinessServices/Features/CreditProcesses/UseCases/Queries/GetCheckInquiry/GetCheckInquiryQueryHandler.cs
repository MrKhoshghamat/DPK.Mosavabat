using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DPK.Mosavabat.BusinessServices.Abstractions.Queries;
using DPK.Mosavabat.ExternalServices.Payam.Contract;
using DPK.Mosavabat.Models.DataModels.Payam;
using DPK.Mosavabat.Models.EntitiesDto.Payam;
using DPK.Mosavabat.Models.Queries.CreditProcesses;

namespace DPK.Mosavabat.BusinessServices.Features.CreditProcesses.UseCases.Queries.GetCheckInquiry;

/// <summary>
/// هندلر ورودی سرویس استعلام چک برگشتی مشتری
/// </summary>
internal class GetCheckInquiryQueryHandler(
    IPayamExternalService payamExternalService,
    IMapper mapper)
    : IQueryHandler<GetCheckInquiryQuery, CheckInquiryDto>
{
    public async Task<CheckInquiryDto> Handle(
        GetCheckInquiryQuery query,
        CancellationToken cancellationToken)
    {
        return await payamExternalService.CheckInquiry(
            mapper.Map<CheckInquiryDm>(query),
            cancellationToken);
    }
}