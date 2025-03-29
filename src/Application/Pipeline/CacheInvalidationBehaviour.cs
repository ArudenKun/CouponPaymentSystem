using Application.Common.Interfaces.Caching;
using MediatR;
using Microsoft.Extensions.Logging;
using ZiggyCreatures.Caching.Fusion;

namespace Application.Pipeline;

public class CacheInvalidationBehaviour<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICacheInvalidatorRequest<TResponse>
{
    private readonly IFusionCache _fusionCache;
    private readonly ILogger<CacheInvalidationBehaviour<TRequest, TResponse>> _logger;

    public CacheInvalidationBehaviour(
        IFusionCache fusionCache,
        ILogger<CacheInvalidationBehaviour<TRequest, TResponse>> logger
    )
    {
        _fusionCache = fusionCache;
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        _logger.LogTrace(
            "Handling request of type {RequestType} with details {@Request}",
            nameof(request),
            request
        );
        var response = await next().ConfigureAwait(false);
        if (!string.IsNullOrEmpty(request.CacheKey))
        {
            await _fusionCache.RemoveAsync(request.CacheKey, token: cancellationToken);
            _logger.LogTrace("Cache key {CacheKey} removed from cache", request.CacheKey);
        }

        if (request.Tags == null || !request.Tags.Any())
            return response;

        foreach (var tag in request.Tags)
        {
            await _fusionCache.RemoveByTagAsync(tag, token: cancellationToken);
            _logger.LogTrace("Cache tag {CacheTag} removed from cache", tag);
        }

        return response;
    }
}
