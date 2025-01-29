using System.Linq.Expressions;

namespace Cdcn.Enterprise.Library.Domain.Data
{
    /// <summary>
    /// Provides extension methods for combining LINQ expressions using logical OR and AND operations.
    /// </summary>
    public static class LinqFilterListExtensions
    {
        /// <summary>
        /// Combines multiple LINQ expressions using a logical OR operation.
        /// </summary>
        /// <typeparam name="T">The type of the entity being filtered.</typeparam>
        /// <param name="filters">An array of LINQ expressions to combine.</param>
        /// <returns>A single LINQ expression representing the combined OR operation.</returns>
        public static Expression<Func<T, bool>> CombineOr<T>(params Expression<Func<T, bool>>[] filters)
        {
            return filters.CombineOr();
        }

        /// <summary>
        /// Combines multiple LINQ expressions using a logical OR operation.
        /// </summary>
        /// <typeparam name="T">The type of the entity being filtered.</typeparam>
        /// <param name="filters">A collection of LINQ expressions to combine.</param>
        /// <returns>A single LINQ expression representing the combined OR operation.</returns>
        public static Expression<Func<T, bool>> CombineOr<T>(this IEnumerable<Expression<Func<T, bool>>> filters)
        {
            switch (filters.Count())
            {
                case 0:
                    {
                        // If no filters are provided, return an expression that always evaluates to true.
                        Expression<Func<T, bool>> alwaysTrue = x => true;
                        return alwaysTrue;
                    }
                case 1:
                    {
                        // If only one filter is provided, return it directly.
                        return filters.First();
                    }
                default:
                    {
                        // Combine multiple filters using a logical OR operation.
                        var result = filters.First();

                        foreach (var next in filters.Skip(1))
                        {
                            // Replace the parameter in the current expression with the parameter from the next expression.
                            var nextExpression = new ReplaceVisitor(result.Parameters[0], next.Parameters[0]).Visit(result.Body);
                            if (nextExpression != null)
                            {
                                // Combine the expressions using OR.
                                result = Expression.Lambda<Func<T, bool>>(Expression.OrElse(nextExpression, next.Body), next.Parameters);
                            }
                        }

                        return result;
                    }
            }
        }

        /// <summary>
        /// Combines multiple LINQ expressions using a logical AND operation.
        /// </summary>
        /// <typeparam name="T">The type of the entity being filtered.</typeparam>
        /// <param name="filters">An array of LINQ expressions to combine.</param>
        /// <returns>A single LINQ expression representing the combined AND operation.</returns>
        public static Expression<Func<T, bool>> CombineAnd<T>(params Expression<Func<T, bool>>[] filters)
        {
            return filters.CombineAnd();
        }

        /// <summary>
        /// Combines multiple LINQ expressions using a logical AND operation.
        /// </summary>
        /// <typeparam name="T">The type of the entity being filtered.</typeparam>
        /// <param name="filters">A collection of LINQ expressions to combine.</param>
        /// <returns>A single LINQ expression representing the combined AND operation.</returns>
        public static Expression<Func<T, bool>> CombineAnd<T>(this IEnumerable<Expression<Func<T, bool>>> filters)
        {
            switch (filters.Count())
            {
                case 0:
                    {
                        // If no filters are provided, return an expression that always evaluates to true.
                        Expression<Func<T, bool>> alwaysTrue = x => true;
                        return alwaysTrue;
                    }
                case 1:
                    {
                        // If only one filter is provided, return it directly.
                        return filters.First();
                    }
                default:
                    {
                        // Combine multiple filters using a logical AND operation.
                        var result = filters.First();

                        foreach (var next in filters.Skip(1))
                        {
                            // Replace the parameter in the current expression with the parameter from the next expression.
                            var nextExpression = new ReplaceVisitor(result.Parameters[0], next.Parameters[0]).Visit(result.Body);

                            if (nextExpression != null)
                            {
                                // Combine the expressions using AND.
                                result = Expression.Lambda<Func<T, bool>>(Expression.AndAlso(nextExpression, next.Body), next.Parameters);
                            }
                        }

                        return result;
                    }
            }
        }

        /// <summary>
        /// A helper class that replaces one expression with another within an expression tree.
        /// </summary>
        private class ReplaceVisitor : ExpressionVisitor
        {
            private readonly Expression from, to;

            /// <summary>
            /// Initializes a new instance of the <see cref="ReplaceVisitor"/> class.
            /// </summary>
            /// <param name="from">The expression to replace.</param>
            /// <param name="to">The expression to replace it with.</param>
            public ReplaceVisitor(Expression from, Expression to)
            {
                this.from = from;
                this.to = to;
            }

            /// <summary>
            /// Visits the expression node and replaces it if it matches the target expression.
            /// </summary>
            /// <param name="node">The expression node to visit.</param>
            /// <returns>The modified expression node.</returns>
            public override Expression? Visit(Expression? node)
            {
                return node == from ? to : base.Visit(node);
            }
        }
    }
}
