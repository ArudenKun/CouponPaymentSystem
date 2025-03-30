using ErrorOr;
using MediatR;

namespace Application.Common.Interfaces;

public interface IAppRequestHandler<in TRequest, TResponse>
    : IRequestHandler<TRequest, ErrorOr<TResponse>>
    where TRequest : IRequest<ErrorOr<TResponse>>;

public interface IAppRequestHandler<in TRequest> : IRequestHandler<TRequest>
    where TRequest : IRequest;
