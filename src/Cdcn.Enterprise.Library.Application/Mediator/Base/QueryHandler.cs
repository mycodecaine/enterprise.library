using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdcn.Enterprise.Library.Application.Mediator.Base
{
    public abstract class QueryHandler<TQuery, TResponse> : BaseHandler<TResponse>, IQueryHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
    {
        protected QueryHandler(ILogger logger) : base(logger)
        {
        }
        public abstract Task<TResponse> Handle(TQuery request, CancellationToken cancellationToken);
    }
}
