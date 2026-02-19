using DPK.Mosavabat.Models.Abstractions.Commands;
using DPK.Mosavabat.Models.Abstractions.Queries;
using System.Threading;
using System.Threading.Tasks;
using DPK.Foundation.Dependency;

namespace DPK.Mosavabat.BusinessServices.Execution;

/// <summary>
/// ماژول اجرای هندلرها
/// </summary>
public interface IExecutionModule : ITransientDependency
{
    /// <summary>
    /// اجرای کامندها با خروجی
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    Task<TResult> ExecuteCommandAsync<TResult>(
        ICommand<TResult> command,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// اجرای کامندها بدون خروجی
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task ExecuteCommandAsync(
        ICommand command,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// اجرای کوئری ها
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    Task<TResult> ExecuteQueryAsync<TResult>(
        IQuery<TResult> query,
        CancellationToken cancellationToken = default);
}