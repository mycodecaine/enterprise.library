using MediatR;

namespace Cdcn.Enterprise.Library.Application.Mediator.Commands
{
    /// <summary>
    /// Represents a command with a response type that implements the IRequest interface.
    /// </summary>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    public interface ICommand<out TResponse> : IRequest<TResponse>
    {
    }
}
