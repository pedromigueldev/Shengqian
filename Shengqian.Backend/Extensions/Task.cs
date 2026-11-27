
using SmartCtor.Monadics;

namespace Shengqian.Backend.Extensions;

public readonly record struct TaskFail(Exception Exception);

public static class TaskFunctionalExtensions
{
    public static async Task<Result<TResult, TaskFail>> UsingAsync<TResource, TResult>(
        this Task<TResource> resourceTask,
        Func<TResource, Task<TResult>> fn)
        where TResource : IAsyncDisposable
        where TResult : notnull
    {
        try
        {
            await using var resource = await resourceTask;
            return await fn(resource);
        }
        catch (Exception ex) when (ex is not OperationCanceledException) { return new TaskFail(ex); }
    }

    public static async Task<Result<Output, TaskFail>> TryMapAsync<Input, Output>(this Task<Input> self, Func<Input, Output> factory)
        where Output : notnull
    {
        try
        {
            return factory(await self);
        }
        catch (Exception ex) when (ex is not OperationCanceledException) { return new TaskFail(ex); }
    }

    public static async Task<U> MatchAsync<InputS, InputE, U>(this Task<Result<InputS, InputE>> self, Func<InputS, U> onSuccess, Func<InputE, U> onError) 
          where InputS : notnull
          where InputE : notnull
    => await self switch
    {
        InputS some => onSuccess(some),
        InputE error => onError(error)
    };
}
