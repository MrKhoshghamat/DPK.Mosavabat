using DPK.Mosavabat.Common.Primitives;
using System.Threading.Tasks;
using System.Threading;
using DPK.Foundation.Dependency;
using DPK.Mosavabat.Models.EntitiesDto.Users;
using DPK.Mosavabat.Models.DataModels.Users;

namespace DPK.Mosavabat.SqlBusinessServices.Users.Contract;

public interface IUserSqlService
    : ITransientDependency
{
    Task<Result<GetUsernameByBranchCodeDto>> GetUsernameByBranchCode(
        GetUsernameByBranchCodeDm dm,
        CancellationToken cancellationToken = default);
}