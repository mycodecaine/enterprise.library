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
    public abstract class CommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
        private readonly ILogger _logger;

        protected CommandHandler(ILogger logger)
        {
            _logger = logger;
        }

        public abstract Task<TResponse> Handle(TCommand request, CancellationToken cancellationToken);
        

        protected async Task<TResponse> HandleSafelyAsync(
            Func<Task<TResponse>> handleCoreAsync, string context = "")
        {
            try
            {
                return await handleCoreAsync();
            }
            catch (ServiceInfrastructureException ex)
            {
                _logger?.LogError(ex, "An ServiceInfrastructureException occurred.");
                throw; // Re throw the original exception.
            }
            catch (EnterpriseLibraryException ex)
            {
                _logger?.LogError(ex, "An EnterpriseLibraryException occurred.");
                throw; // Re throw the original exception.
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "An ServiceApplicationException occurred.");
                throw HandleException.ThrowServiceApplicationException(ex, _logger, $"{context ?? this.GetType().FullName}");
            }
        }
    }
}
