using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DPK.Mosavabat.BusinessServices.Abstractions.Queries;
using DPK.Mosavabat.Common.Extensions;
using DPK.Mosavabat.Common.Primitives;
using DPK.Mosavabat.ExternalServices.Lcp.Contract;
using DPK.Mosavabat.Models.DataModels.Lcp;
using DPK.Mosavabat.Models.EntitiesDto.Lcp;
using DPK.Mosavabat.Models.Queries.Lcp;

namespace DPK.Mosavabat.BusinessServices.Features.Lcp.UseCases.Queries.GetBoardMembers;

/// <summary>
/// هندلر سرویس دریافت لیست اعضای هیئت مدیره
/// </summary>
internal class GetBoardMembersQueryHandler(
    ILcpExternalService lcpExternalService,
    IMapper mapper)
    : IQueryHandler<
        GetBoardMembersQuery,
        Result<List<BoardMemberDto>>>
{
    public async Task<Result<List<BoardMemberDto>>> Handle(
        GetBoardMembersQuery query,
        CancellationToken cancellationToken)
    {
        return await Result.Success(query)
            .Map(mapper.Map<GetBoardMembersDm>)
            .Bind(dm => lcpExternalService.GetBoardMembers(
                dm,
                cancellationToken));
    }
}