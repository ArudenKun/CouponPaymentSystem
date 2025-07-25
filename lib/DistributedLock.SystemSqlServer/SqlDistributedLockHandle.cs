using Medallion.Threading.Internal;

namespace Medallion.Threading.SystemSqlServer;

/// <summary>
/// Implements <see cref="IDistributedSynchronizationHandle"/>
/// </summary>
public sealed class SqlDistributedLockHandle : IDistributedSynchronizationHandle
{
    private IDistributedSynchronizationHandle? _innerHandle;

    internal SqlDistributedLockHandle(IDistributedSynchronizationHandle innerHandle)
    {
        this._innerHandle = innerHandle;
    }

    /// <summary>
    /// Implements <see cref="IDistributedSynchronizationHandle.HandleLostToken"/>
    /// </summary>
    public CancellationToken HandleLostToken =>
        this._innerHandle?.HandleLostToken ?? throw this.ObjectDisposed();

    /// <summary>
    /// Releases the lock
    /// </summary>
    public void Dispose() => Interlocked.Exchange(ref this._innerHandle, null)?.Dispose();

    /// <summary>
    /// Releases the lock asynchronously
    /// </summary>
    public ValueTask DisposeAsync() =>
        Interlocked.Exchange(ref this._innerHandle, null)?.DisposeAsync() ?? default;
}
