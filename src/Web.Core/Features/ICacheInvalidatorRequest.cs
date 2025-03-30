namespace Web.Core.Features;

public interface ICacheInvalidatorRequest<TResponse> : IAppRequest<TResponse>
{
    string CacheKey { get; }
    IEnumerable<string>? Tags { get; }
}
