using Cdcn.Enterprise.Library.Domain.Primitives.Errors;

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


        /// <summary>
        /// Processes the result of a <see cref="Maybe{TIn}"/> asynchronously and returns a transformed value
        /// based on whether the operation succeeded or failed.
        /// </summary>
        /// <typeparam name="TIn">The type of the input value contained in the <see cref="Maybe{TIn}"/>.</typeparam>
        /// <typeparam name="TOut">The type of the output value returned by the match functions.</typeparam>
        /// <param name="resultTask">A task that resolves to a <see cref="Maybe{TIn}"/>.</param>
        /// <param name="onSuccess">
        /// A function to execute if the <see cref="Maybe{TIn}"/> contains a value. 
        /// It takes the value as input and returns a result of type <typeparamref name="TOut"/>.
        /// </param>
        /// <param name="onFailure">
        /// A function to execute if the <see cref="Maybe{TIn}"/> does not contain a value. 
        /// It takes an <see cref="Error"/> as input and returns a result of type <typeparamref name="TOut"/>.
        /// </param>
        /// <returns>
        /// A task that resolves to a value of type <typeparamref name="TOut"/>, determined by the outcome of the <see cref="Maybe{TIn}"/>.
        /// </returns>
        /// <remarks>
        /// This method simplifies handling of the <see cref="Maybe{TIn}"/> by providing separate paths for success and failure scenarios,
        /// ensuring clean and expressive handling of optional results.
        /// </remarks>
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
