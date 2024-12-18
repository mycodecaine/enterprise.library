﻿using Cdcn.Enterprise.Library.Domain.Primitives;

namespace Cdcn.Enterprise.Library.Domain.Primitives.Maybe
{
    public static class MaybeExtensions
    {
        /// <summary>
        /// Binds to the result of the function and returns it.
        /// </summary>
        /// <typeparam name="TIn">The result type.</typeparam>
        /// <typeparam name="TOut">The output result type.</typeparam>
        /// <param name="maybe">The result.</param>
        /// <param name="func">The bind function.</param>
        /// <returns>
        /// The success result with the bound value if the current result is a success result, otherwise a failure result.
        /// </returns>
        public static async Task<Maybe<TOut>> Bind<TIn, TOut>(this Maybe<TIn> maybe, Func<TIn, Task<Maybe<TOut>>> func) =>
            maybe.HasValue ? await func(maybe.Value) : Maybe<TOut>.None;

       

        public static async Task<TOut> Match<TIn, TOut>(
            this Task<Maybe<TIn>> resultTask,
            Func<TIn, TOut> onSuccess,
            Func<Error, TOut> onFailure)
        {
            Maybe<TIn> maybe = await resultTask;

            return maybe.HasValue ? onSuccess(maybe.Value) : onFailure(maybe.Error);
        }
    }
}
