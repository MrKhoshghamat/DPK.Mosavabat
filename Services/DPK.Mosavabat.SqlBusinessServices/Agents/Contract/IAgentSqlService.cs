using System.Threading;
using System.Threading.Tasks;
using DPK.Foundation.Dependency;
using DPK.Mosavabat.Common.Primitives;
using DPK.Mosavabat.Models.DataModels.Agents;
using DPK.Mosavabat.Models.EntitiesDto.Agents;

namespace DPK.Mosavabat.SqlBusinessServices.Agents.Contract;

public interface IAgentSqlService
    : ITransientDependency
{
    Task<Result<UpdateCaseIdAndRadNumberInAgentRegistryDto>> UpdateCaseIdAndRadNumberInAgentRegistry(
        UpdateCaseIdAndRadNumberInAgentRegistryDm dm,
        CancellationToken cancellationToken = default);

    Task<Result<GetUserNameByCaseIdDto>> GetUserNameByCaseId(
        GetUserNameByCaseIdDm dm,
        CancellationToken cancellationToken = default);

    Task<Result<GetUserIdByUsernameDto>> GetUserIdByUsername(
        GetUserIdByUsernameDm dm,
        CancellationToken cancellationToken = default);

    Task<Result<GetRadNumberByCaseIdDto>> GetRadNumberByCaseId(
        GetRadNumberByCaseIdDm dm,
        CancellationToken cancellationToken = default);

    Task<Result<AbortAgentCaseDto>> AbortAgentCase(
        AbortAgentCaseDm dm,
        CancellationToken cancellationToken = default);
}