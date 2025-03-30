using ErrorOr;
using MediatR;

namespace Application.Common.Interfaces;

public interface IAppRequest<TResponse> : IRequest<ErrorOr<TResponse>>;

public interface IAppRequest : IRequest;
