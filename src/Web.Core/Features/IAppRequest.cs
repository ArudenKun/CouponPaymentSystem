using ErrorOr;
using MediatR;

namespace Web.Core.Features;

public interface IAppRequest<TResponse> : IRequest<ErrorOr<TResponse>>;

public interface IAppRequest : IRequest;
