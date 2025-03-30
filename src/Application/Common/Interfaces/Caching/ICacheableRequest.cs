using MediatR;

namespace Application.Common.Interfaces.Caching;

public interface ICacheableRequest<TResponse> : IAppRequest<TResponse>
{
    string CacheKey { get; }
    IEnumerable<string>? Tags { get; }
}
