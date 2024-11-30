using Cdcn.Enterprise.Library.Application.Exceptions;
using Cdcn.Enterprise.Library.Domain.Exceptions;
using Cdcn.Enterprise.Library.Domain.Primitives.Result;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdcn.Enterprise.Library.Application.Mediator.Base
{
    public abstract class CommandHandler<TCommand, TResponse>  : BaseHandler<TResponse>, ICommandHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
        
        protected CommandHandler(ILogger logger):base(logger) 
        {
           
        }

        public abstract Task<TResponse> Handle(TCommand request, CancellationToken cancellationToken);        

        
    }
}
