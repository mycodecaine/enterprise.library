using MediatR;

namespace Cdcn.Enterprise.Library.Application.Mediator
{
    public interface ICommand<out TResponse> : IRequest<TResponse>
    {
    }
}
