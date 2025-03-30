using ErrorOr;
using MediatR;

namespace Web.Core.Features;

public interface IAppRequestHandler<in TRequest, TResponse>
    : IRequestHandler<TRequest, ErrorOr<TResponse>>
    where TRequest : IRequest<ErrorOr<TResponse>>;

public interface IAppRequestHandler<in TRequest> : IRequestHandler<TRequest>
    where TRequest : IRequest;
