using System.Threading;
using System.Threading.Tasks;
using DPK.Mosavabat.Models.Abstractions.Commands;
using DPK.Mosavabat.Models.Abstractions.Queries;
using MediatR;

namespace DPK.Mosavabat.BusinessServices.Execution;

/// <summary>
/// ماژول اجرای هندلرها
/// </summary>
public class ExecutionModule(
    IMediator mediator)
    : IExecutionModule
{
    /// <summary>
    /// اجرای کامندها با خروجی
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    public async Task<TResult> ExecuteCommandAsync<TResult>(
        ICommand<TResult> command,
        CancellationToken cancellationToken = default)
    {
        return await mediator.Send(
            command,
            cancellationToken);
    }

    /// <summary>
    /// اجرای کامندها بدون خروجی
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task ExecuteCommandAsync(
        ICommand command,
        CancellationToken cancellationToken = default)
    {
        await mediator.Send(
            command,
            cancellationToken);
    }

    /// <summary>
    /// اجرای کوئری ها
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    public async Task<TResult> ExecuteQueryAsync<TResult>(
        IQuery<TResult> query,
        CancellationToken cancellationToken = default)
    {
        return await mediator.Send(
            query,
            cancellationToken);
    }
}