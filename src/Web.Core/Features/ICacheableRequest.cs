namespace Web.Core.Features;

public interface ICacheableRequest<TResponse> : IAppRequest<TResponse>
{
    string CacheKey { get; }
    IEnumerable<string>? Tags { get; }
}
