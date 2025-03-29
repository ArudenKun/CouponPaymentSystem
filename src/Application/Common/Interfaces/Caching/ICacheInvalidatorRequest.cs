﻿using MediatR;

namespace Application.Common.Interfaces.Caching;

public interface ICacheInvalidatorRequest<out TResponse> : IRequest<TResponse>
{
    string CacheKey { get; }
    IEnumerable<string>? Tags { get; }
}
