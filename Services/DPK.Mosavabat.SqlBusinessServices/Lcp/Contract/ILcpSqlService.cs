using System.Threading;
using System.Threading.Tasks;
using DPK.Foundation.Dependency;
using DPK.Mosavabat.Models.DataModels.Lcp;
using DPK.Mosavabat.Models.EntitiesDto.Lcp;

namespace DPK.Mosavabat.SqlBusinessServices.Lcp.Contract;

public interface ILcpSqlService
    : ITransientDependency
{
    Task<InsertLcpRequestFacilityStatusDto> InsertRequestFacilityStatus(
        InsertLcpRequestFacilityStatusDm dm,
        CancellationToken cancellationToken);
}