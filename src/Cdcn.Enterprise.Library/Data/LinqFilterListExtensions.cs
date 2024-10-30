using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Cdcn.Enterprise.Library.Domain.Data
{
    public static class LinqFilterListExtensions
    {
        public static Expression<Func<T, bool>> CombineOr<T>(params Expression<Func<T, bool>>[] filters)
        {
            return filters.CombineOr();
        }

        public static Expression<Func<T, bool>> CombineOr<T>(this IEnumerable<Expression<Func<T, bool>>> filters)
        {
            switch (filters.Count())
            {
                case 0:
                    {
                        Expression<Func<T, bool>> alwaysTrue = x => true;
                        return alwaysTrue;
                    }
                case 1:
                    {
                        return filters.First();
                    }
                default:
                    {
                        var result = filters.First();

                        foreach (var next in filters.Skip(1))
                        {
                            var nextExpression = new ReplaceVisitor(result.Parameters[0], next.Parameters[0]).Visit(result.Body);
                            if (nextExpression != null)
                            {
                                result = Expression.Lambda<Func<T, bool>>(Expression.OrElse(nextExpression, next.Body), next.Parameters);
                            }
                        }

                        return result;
                    }
            }
        }

        public static Expression<Func<T, bool>> CombineAnd<T>(params Expression<Func<T, bool>>[] filters)
        {
            return filters.CombineAnd();
        }

        public static Expression<Func<T, bool>> CombineAnd<T>(this IEnumerable<Expression<Func<T, bool>>> filters)
        {
            switch (filters.Count())
            {
                case 0:
                    {
                        Expression<Func<T, bool>> alwaysTrue = x => true;
                        return alwaysTrue;
                    }
                case 1:
                    {
                        return filters.First();
                    }
                default:
                    {
                        var result = filters.First();

                        foreach (var next in filters.Skip(1))
                        {
                            var nextExpression = new ReplaceVisitor(result.Parameters[0], next.Parameters[0]).Visit(result.Body);

                            if (nextExpression != null)
                            {
                                result = Expression.Lambda<Func<T, bool>>(Expression.OrElse(nextExpression, next.Body), next.Parameters);
                            }
                        }

                        return result;
                    }
            }
        }

        class ReplaceVisitor : ExpressionVisitor
        {
            private readonly Expression from, to;

            public ReplaceVisitor(Expression from, Expression to)
            {
                this.from = from;
                this.to = to;
            }

            public override Expression? Visit(Expression? node)
            {
                return node == from ? to : base.Visit(node);
            }
        }
    }
}
