using System.Threading.Tasks;
using DPK.Mosavabat.Common.Primitives;

namespace DPK.Mosavabat.Common.Extensions;

public static class ResultExtensions
{
    // ================================================================
    // BIND – ASYNC (IO / External / SQL)
    // ================================================================

    public static async Task<Result<TOut>> Bind<TIn, TOut>(
        this Task<Result<TIn>> resultTask,
        Func<TIn, Task<Result<TOut>>> next)
    {
        var result = await resultTask.ConfigureAwait(false);

        if (result.IsFailure)
            return Result.Failure<TOut>(result.Error);

        return await next(result.Value).ConfigureAwait(false);
    }

    public static async Task<Result<TOut>> Bind<TIn, TOut>(
        this Result<TIn> result,
        Func<TIn, Task<Result<TOut>>> next)
    {
        if (result.IsFailure)
            return Result.Failure<TOut>(result.Error);

        return await next(result.Value).ConfigureAwait(false);
    }

    // ================================================================
    // BIND – SYNC (RULE / ENSURE)
    // ================================================================

    public static async Task<Result<TOut>> Bind<TIn, TOut>(
        this Task<Result<TIn>> resultTask,
        Func<TIn, Result<TOut>> rule)
    {
        var result = await resultTask.ConfigureAwait(false);

        if (result.IsFailure)
            return Result.Failure<TOut>(result.Error);

        return rule(result.Value);
    }

    public static Result<TOut> Bind<TIn, TOut>(
        this Result<TIn> result,
        Func<TIn, Result<TOut>> rule)
    {
        if (result.IsFailure)
            return Result.Failure<TOut>(result.Error);

        return rule(result.Value);
    }

    // ================================================================
    // MAP – TRANSFORMATION (NO FAILURE CREATION)
    // ================================================================

    public static async Task<Result<TOut>> Map<TIn, TOut>(
        this Task<Result<TIn>> resultTask,
        Func<TIn, TOut> mapper)
    {
        var result = await resultTask.ConfigureAwait(false);

        return result.IsFailure
            ? Result.Failure<TOut>(result.Error)
            : Result.Success(mapper(result.Value));
    }

    public static Result<TOut> Map<TIn, TOut>(
        this Result<TIn> result,
        Func<TIn, TOut> mapper)
    {
        return result.IsFailure
            ? Result.Failure<TOut>(result.Error)
            : Result.Success(mapper(result.Value));
    }

    // ================================================================
    // TAP – SIDE EFFECT (WITH RESULT)
    // ================================================================

    public static async Task<Result<TIn>> Tap<TIn>(
        this Task<Result<TIn>> resultTask,
        Func<TIn, Task<Result>> sideEffect)
    {
        var result = await resultTask.ConfigureAwait(false);

        if (result.IsFailure)
            return result;

        var sideEffectResult = await sideEffect(result.Value).ConfigureAwait(false);

        return sideEffectResult.IsFailure
            ? Result.Failure<TIn>(sideEffectResult.Error)
            : result;
    }

    public static async Task<Result<TIn>> Tap<TIn>(
        this Result<TIn> result,
        Func<TIn, Task<Result>> sideEffect)
    {
        if (result.IsFailure)
            return result;

        var sideEffectResult = await sideEffect(result.Value).ConfigureAwait(false);

        return sideEffectResult.IsFailure
            ? Result.Failure<TIn>(sideEffectResult.Error)
            : result;
    }

    // ================================================================
    // TAP – SIDE EFFECT (FIRE & FORGET – OPTIONAL)
    // ================================================================

    public static async Task<Result<TIn>> Tap<TIn>(
        this Task<Result<TIn>> resultTask,
        Func<TIn, Task> sideEffect)
    {
        var result = await resultTask.ConfigureAwait(false);

        if (result.IsSuccess)
            await sideEffect(result.Value).ConfigureAwait(false);

        return result;
    }
}
