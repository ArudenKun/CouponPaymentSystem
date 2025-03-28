using System.Data;
using System.Linq.Expressions;
using Application.Common.Interfaces;
using Domain.Common.Entities;
using Domain.Common.Enums;
using MicroOrm.Dapper.Repositories;
using MicroOrm.Dapper.Repositories.SqlGenerator.Filters;

namespace Infrastructure.Data;

internal sealed class Repository<TId, TEntity>
    : DapperRepository<TEntity>,
        IRepository<TId, TEntity>
    where TId : notnull
    where TEntity : class, IEntity<TId>
{
    public Repository(IDbConnection connection)
        : base(connection) { }

    public TEntity? FindById(TId id)
    {
        return base.FindById(id);
    }

    public Task<TEntity?> FindByIdAsync(TId id)
    {
        return base.FindByIdAsync(id);
    }

    public Task<TEntity?> FindByIdAsync(TId id, CancellationToken cancellationToken)
    {
        return base.FindByIdAsync(id, cancellationToken);
    }

    public TEntity? FindById(TId id, IDbTransaction? transaction)
    {
        return base.FindById(id, transaction);
    }

    public Task<TEntity?> FindByIdAsync(TId id, IDbTransaction? transaction)
    {
        return base.FindByIdAsync(id, transaction);
    }

    public Task<TEntity?> FindByIdAsync(
        TId id,
        IDbTransaction? transaction,
        CancellationToken cancellationToken
    )
    {
        return base.FindByIdAsync(id, transaction, cancellationToken);
    }

    public TEntity? FindById<TChild1>(TId id, Expression<Func<TEntity, object>> tChild1)
    {
        return base.FindById<TChild1>(id, tChild1);
    }

    public Task<TEntity?> FindByIdAsync<TChild1>(TId id, Expression<Func<TEntity, object>> tChild1)
    {
        return base.FindByIdAsync<TChild1>(id, tChild1);
    }

    public Task<TEntity?> FindByIdAsync<TChild1>(
        TId id,
        Expression<Func<TEntity, object>> tChild1,
        CancellationToken cancellationToken
    )
    {
        return base.FindByIdAsync<TChild1>(id, tChild1, cancellationToken);
    }

    public TEntity? FindById<TChild1>(
        TId id,
        Expression<Func<TEntity, object>> tChild1,
        IDbTransaction? transaction
    )
    {
        return base.FindById<TChild1>(id, tChild1, transaction);
    }

    public Task<TEntity?> FindByIdAsync<TChild1>(
        TId id,
        Expression<Func<TEntity, object>> tChild1,
        IDbTransaction? transaction
    )
    {
        return base.FindByIdAsync<TChild1>(id, tChild1, transaction);
    }

    public Task<TEntity?> FindByIdAsync<TChild1>(
        TId id,
        Expression<Func<TEntity, object>> tChild1,
        IDbTransaction? transaction,
        CancellationToken cancellationToken
    )
    {
        return base.FindByIdAsync<TChild1>(id, tChild1, transaction, cancellationToken);
    }

    public TEntity? FindById<TChild1, TChild2>(
        TId id,
        Expression<Func<TEntity, object>> tChild1,
        Expression<Func<TEntity, object>> tChild2
    )
    {
        return base.FindById<TChild1, TChild2>(id, tChild1, tChild2);
    }

    public Task<TEntity?> FindByIdAsync<TChild1, TChild2>(
        TId id,
        Expression<Func<TEntity, object>> tChild1,
        Expression<Func<TEntity, object>> tChild2
    )
    {
        return base.FindByIdAsync<TChild1, TChild2>(id, tChild1, tChild2);
    }

    public Task<TEntity?> FindByIdAsync<TChild1, TChild2>(
        TId id,
        Expression<Func<TEntity, object>> tChild1,
        Expression<Func<TEntity, object>> tChild2,
        CancellationToken cancellationToken
    )
    {
        return base.FindByIdAsync<TChild1, TChild2>(id, tChild1, tChild2, cancellationToken);
    }

    public TEntity? FindById<TChild1, TChild2>(
        TId id,
        Expression<Func<TEntity, object>> tChild1,
        Expression<Func<TEntity, object>> tChild2,
        IDbTransaction? transaction
    )
    {
        return base.FindById<TChild1, TChild2>(id, tChild1, tChild2, transaction);
    }

    public Task<TEntity?> FindByIdAsync<TChild1, TChild2>(
        TId id,
        Expression<Func<TEntity, object>> tChild1,
        Expression<Func<TEntity, object>> tChild2,
        IDbTransaction? transaction
    )
    {
        return base.FindByIdAsync<TChild1, TChild2>(id, tChild1, tChild2, transaction);
    }

    public Task<TEntity?> FindByIdAsync<TChild1, TChild2>(
        TId id,
        Expression<Func<TEntity, object>> tChild1,
        Expression<Func<TEntity, object>> tChild2,
        IDbTransaction? transaction,
        CancellationToken cancellationToken
    )
    {
        return base.FindByIdAsync<TChild1, TChild2>(
            id,
            tChild1,
            tChild2,
            transaction,
            cancellationToken
        );
    }

    public TEntity? FindById<TChild1, TChild2, TChild3>(
        TId id,
        Expression<Func<TEntity, object>> tChild1,
        Expression<Func<TEntity, object>> tChild2,
        Expression<Func<TEntity, object>> tChild3
    )
    {
        return base.FindById<TChild1, TChild2, TChild3>(id, tChild1, tChild2, tChild3);
    }

    public Task<TEntity?> FindByIdAsync<TChild1, TChild2, TChild3>(
        TId id,
        Expression<Func<TEntity, object>> tChild1,
        Expression<Func<TEntity, object>> tChild2,
        Expression<Func<TEntity, object>> tChild3
    )
    {
        return base.FindByIdAsync<TChild1, TChild2, TChild3>(id, tChild1, tChild2, tChild3);
    }

    public Task<TEntity?> FindByIdAsync<TChild1, TChild2, TChild3>(
        TId id,
        Expression<Func<TEntity, object>> tChild1,
        Expression<Func<TEntity, object>> tChild2,
        Expression<Func<TEntity, object>> tChild3,
        CancellationToken cancellationToken
    )
    {
        return base.FindByIdAsync<TChild1, TChild2, TChild3>(
            id,
            tChild1,
            tChild2,
            tChild3,
            cancellationToken
        );
    }

    public TEntity? FindById<TChild1, TChild2, TChild3>(
        TId id,
        Expression<Func<TEntity, object>> tChild1,
        Expression<Func<TEntity, object>> tChild2,
        Expression<Func<TEntity, object>> tChild3,
        IDbTransaction? transaction
    )
    {
        return base.FindById<TChild1, TChild2, TChild3>(id, tChild1, tChild2, tChild3, transaction);
    }

    public Task<TEntity?> FindByIdAsync<TChild1, TChild2, TChild3>(
        TId id,
        Expression<Func<TEntity, object>> tChild1,
        Expression<Func<TEntity, object>> tChild2,
        Expression<Func<TEntity, object>> tChild3,
        IDbTransaction? transaction
    )
    {
        return base.FindByIdAsync<TChild1, TChild2, TChild3>(
            id,
            tChild1,
            tChild2,
            tChild3,
            transaction
        );
    }

    public Task<TEntity?> FindByIdAsync<TChild1, TChild2, TChild3>(
        TId id,
        Expression<Func<TEntity, object>> tChild1,
        Expression<Func<TEntity, object>> tChild2,
        Expression<Func<TEntity, object>> tChild3,
        IDbTransaction? transaction,
        CancellationToken cancellationToken
    )
    {
        return base.FindByIdAsync<TChild1, TChild2, TChild3>(
            id,
            tChild1,
            tChild2,
            tChild3,
            transaction,
            cancellationToken
        );
    }

    public TEntity? FindById<TChild1, TChild2, TChild3, TChild4>(
        TId id,
        Expression<Func<TEntity, object>> tChild1,
        Expression<Func<TEntity, object>> tChild2,
        Expression<Func<TEntity, object>> tChild3,
        Expression<Func<TEntity, object>> tChild4
    )
    {
        return base.FindById<TChild1, TChild2, TChild3, TChild4>(
            id,
            tChild1,
            tChild2,
            tChild3,
            tChild4
        );
    }

    public Task<TEntity?> FindByIdAsync<TChild1, TChild2, TChild3, TChild4>(
        TId id,
        Expression<Func<TEntity, object>> tChild1,
        Expression<Func<TEntity, object>> tChild2,
        Expression<Func<TEntity, object>> tChild3,
        Expression<Func<TEntity, object>> tChild4
    )
    {
        return base.FindByIdAsync<TChild1, TChild2, TChild3, TChild4>(
            id,
            tChild1,
            tChild2,
            tChild3,
            tChild4
        );
    }

    public Task<TEntity?> FindByIdAsync<TChild1, TChild2, TChild3, TChild4>(
        TId id,
        Expression<Func<TEntity, object>> tChild1,
        Expression<Func<TEntity, object>> tChild2,
        Expression<Func<TEntity, object>> tChild3,
        Expression<Func<TEntity, object>> tChild4,
        CancellationToken cancellationToken
    )
    {
        return base.FindByIdAsync<TChild1, TChild2, TChild3, TChild4>(
            id,
            tChild1,
            tChild2,
            tChild3,
            tChild4,
            cancellationToken
        );
    }

    public TEntity? FindById<TChild1, TChild2, TChild3, TChild4>(
        TId id,
        Expression<Func<TEntity, object>> tChild1,
        Expression<Func<TEntity, object>> tChild2,
        Expression<Func<TEntity, object>> tChild3,
        Expression<Func<TEntity, object>> tChild4,
        IDbTransaction? transaction
    )
    {
        return base.FindById<TChild1, TChild2, TChild3, TChild4>(
            id,
            tChild1,
            tChild2,
            tChild3,
            tChild4,
            transaction
        );
    }

    public Task<TEntity?> FindByIdAsync<TChild1, TChild2, TChild3, TChild4>(
        TId id,
        Expression<Func<TEntity, object>> tChild1,
        Expression<Func<TEntity, object>> tChild2,
        Expression<Func<TEntity, object>> tChild3,
        Expression<Func<TEntity, object>> tChild4,
        IDbTransaction? transaction
    )
    {
        return base.FindByIdAsync<TChild1, TChild2, TChild3, TChild4>(
            id,
            tChild1,
            tChild2,
            tChild3,
            tChild4,
            transaction
        );
    }

    public Task<TEntity?> FindByIdAsync<TChild1, TChild2, TChild3, TChild4>(
        TId id,
        Expression<Func<TEntity, object>> tChild1,
        Expression<Func<TEntity, object>> tChild2,
        Expression<Func<TEntity, object>> tChild3,
        Expression<Func<TEntity, object>> tChild4,
        IDbTransaction? transaction,
        CancellationToken cancellationToken
    )
    {
        return base.FindByIdAsync<TChild1, TChild2, TChild3, TChild4>(
            id,
            tChild1,
            tChild2,
            tChild3,
            tChild4,
            transaction,
            cancellationToken
        );
    }

    public TEntity? FindById<TChild1, TChild2, TChild3, TChild4, TChild5>(
        TId id,
        Expression<Func<TEntity, object>> tChild1,
        Expression<Func<TEntity, object>> tChild2,
        Expression<Func<TEntity, object>> tChild3,
        Expression<Func<TEntity, object>> tChild4,
        Expression<Func<TEntity, object>> tChild5
    )
    {
        return base.FindById<TChild1, TChild2, TChild3, TChild4, TChild5>(
            id,
            tChild1,
            tChild2,
            tChild3,
            tChild4,
            tChild5
        );
    }

    public Task<TEntity?> FindByIdAsync<TChild1, TChild2, TChild3, TChild4, TChild5>(
        TId id,
        Expression<Func<TEntity, object>> tChild1,
        Expression<Func<TEntity, object>> tChild2,
        Expression<Func<TEntity, object>> tChild3,
        Expression<Func<TEntity, object>> tChild4,
        Expression<Func<TEntity, object>> tChild5
    )
    {
        return base.FindByIdAsync<TChild1, TChild2, TChild3, TChild4, TChild5>(
            id,
            tChild1,
            tChild2,
            tChild3,
            tChild4,
            tChild5
        );
    }

    public Task<TEntity?> FindByIdAsync<TChild1, TChild2, TChild3, TChild4, TChild5>(
        TId id,
        Expression<Func<TEntity, object>> tChild1,
        Expression<Func<TEntity, object>> tChild2,
        Expression<Func<TEntity, object>> tChild3,
        Expression<Func<TEntity, object>> tChild4,
        Expression<Func<TEntity, object>> tChild5,
        CancellationToken cancellationToken
    )
    {
        return base.FindByIdAsync<TChild1, TChild2, TChild3, TChild4, TChild5>(
            id,
            tChild1,
            tChild2,
            tChild3,
            tChild4,
            tChild5,
            cancellationToken
        );
    }

    public TEntity? FindById<TChild1, TChild2, TChild3, TChild4, TChild5>(
        TId id,
        Expression<Func<TEntity, object>> tChild1,
        Expression<Func<TEntity, object>> tChild2,
        Expression<Func<TEntity, object>> tChild3,
        Expression<Func<TEntity, object>> tChild4,
        Expression<Func<TEntity, object>> tChild5,
        IDbTransaction? transaction
    )
    {
        return base.FindById<TChild1, TChild2, TChild3, TChild4, TChild5>(
            id,
            tChild1,
            tChild2,
            tChild3,
            tChild4,
            tChild5,
            transaction
        );
    }

    public Task<TEntity?> FindByIdAsync<TChild1, TChild2, TChild3, TChild4, TChild5>(
        TId id,
        Expression<Func<TEntity, object>> tChild1,
        Expression<Func<TEntity, object>> tChild2,
        Expression<Func<TEntity, object>> tChild3,
        Expression<Func<TEntity, object>> tChild4,
        Expression<Func<TEntity, object>> tChild5,
        IDbTransaction? transaction
    )
    {
        return base.FindByIdAsync<TChild1, TChild2, TChild3, TChild4, TChild5>(
            id,
            tChild1,
            tChild2,
            tChild3,
            tChild4,
            tChild5,
            transaction
        );
    }

    public Task<TEntity?> FindByIdAsync<TChild1, TChild2, TChild3, TChild4, TChild5>(
        TId id,
        Expression<Func<TEntity, object>> tChild1,
        Expression<Func<TEntity, object>> tChild2,
        Expression<Func<TEntity, object>> tChild3,
        Expression<Func<TEntity, object>> tChild4,
        Expression<Func<TEntity, object>> tChild5,
        IDbTransaction? transaction,
        CancellationToken cancellationToken
    )
    {
        return base.FindByIdAsync<TChild1, TChild2, TChild3, TChild4, TChild5>(
            id,
            tChild1,
            tChild2,
            tChild3,
            tChild4,
            tChild5,
            transaction,
            cancellationToken
        );
    }

    public TEntity? FindById<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(
        TId id,
        Expression<Func<TEntity, object>> tChild1,
        Expression<Func<TEntity, object>> tChild2,
        Expression<Func<TEntity, object>> tChild3,
        Expression<Func<TEntity, object>> tChild4,
        Expression<Func<TEntity, object>> tChild5,
        Expression<Func<TEntity, object>> tChild6
    )
    {
        return base.FindById<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(
            id,
            tChild1,
            tChild2,
            tChild3,
            tChild4,
            tChild5,
            tChild6
        );
    }

    public Task<TEntity?> FindByIdAsync<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(
        TId id,
        Expression<Func<TEntity, object>> tChild1,
        Expression<Func<TEntity, object>> tChild2,
        Expression<Func<TEntity, object>> tChild3,
        Expression<Func<TEntity, object>> tChild4,
        Expression<Func<TEntity, object>> tChild5,
        Expression<Func<TEntity, object>> tChild6
    )
    {
        return base.FindByIdAsync<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(
            id,
            tChild1,
            tChild2,
            tChild3,
            tChild4,
            tChild5,
            tChild6
        );
    }

    public Task<TEntity?> FindByIdAsync<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(
        TId id,
        Expression<Func<TEntity, object>> tChild1,
        Expression<Func<TEntity, object>> tChild2,
        Expression<Func<TEntity, object>> tChild3,
        Expression<Func<TEntity, object>> tChild4,
        Expression<Func<TEntity, object>> tChild5,
        Expression<Func<TEntity, object>> tChild6,
        CancellationToken cancellationToken
    )
    {
        return base.FindByIdAsync<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(
            id,
            tChild1,
            tChild2,
            tChild3,
            tChild4,
            tChild5,
            tChild6,
            cancellationToken
        );
    }

    public TEntity? FindById<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(
        TId id,
        Expression<Func<TEntity, object>> tChild1,
        Expression<Func<TEntity, object>> tChild2,
        Expression<Func<TEntity, object>> tChild3,
        Expression<Func<TEntity, object>> tChild4,
        Expression<Func<TEntity, object>> tChild5,
        Expression<Func<TEntity, object>> tChild6,
        IDbTransaction? transaction
    )
    {
        return base.FindById<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(
            id,
            tChild1,
            tChild2,
            tChild3,
            tChild4,
            tChild5,
            tChild6,
            transaction
        );
    }

    public Task<TEntity?> FindByIdAsync<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(
        TId id,
        Expression<Func<TEntity, object>> tChild1,
        Expression<Func<TEntity, object>> tChild2,
        Expression<Func<TEntity, object>> tChild3,
        Expression<Func<TEntity, object>> tChild4,
        Expression<Func<TEntity, object>> tChild5,
        Expression<Func<TEntity, object>> tChild6,
        IDbTransaction? transaction
    )
    {
        return base.FindByIdAsync<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(
            id,
            tChild1,
            tChild2,
            tChild3,
            tChild4,
            tChild5,
            tChild6,
            transaction
        );
    }

    public Task<TEntity?> FindByIdAsync<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(
        TId id,
        Expression<Func<TEntity, object>> tChild1,
        Expression<Func<TEntity, object>> tChild2,
        Expression<Func<TEntity, object>> tChild3,
        Expression<Func<TEntity, object>> tChild4,
        Expression<Func<TEntity, object>> tChild5,
        Expression<Func<TEntity, object>> tChild6,
        IDbTransaction? transaction,
        CancellationToken cancellationToken
    )
    {
        return base.FindByIdAsync<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(
            id,
            tChild1,
            tChild2,
            tChild3,
            tChild4,
            tChild5,
            tChild6,
            transaction,
            cancellationToken
        );
    }

    public new IReadRepository<TId, TEntity> SetSelect<T>(Expression<Func<T, object>> expr)
    {
        base.SetSelect(expr);
        return this;
    }

    public new IReadRepository<TId, TEntity> SetSelect<T>(
        Expression<Func<T, object>> expr,
        bool permanent
    )
    {
        base.SetSelect(expr, permanent);
        return this;
    }

    public new IReadRepository<TId, TEntity> SetSelect(params string[] customSelect)
    {
        base.SetSelect(customSelect);
        return this;
    }

    public new IReadRepository<TId, TEntity> SetSelect(Expression<Func<TEntity, object>> expr)
    {
        base.SetSelect(expr);
        return this;
    }

    public new IReadRepository<TId, TEntity> SetOrderBy()
    {
        base.SetOrderBy();
        return this;
    }

    public IReadRepository<TId, TEntity> SetOrderBy(OrderBy orderBy, params string[] cols)
    {
        base.SetOrderBy(GetSortDirection(orderBy), cols);
        return this;
    }

    public new IReadRepository<TId, TEntity> SetOrderBy(string query)
    {
        base.SetOrderBy(query);
        return this;
    }

    public new IReadRepository<TId, TEntity> SetOrderBy(string query, bool permanent)
    {
        base.SetOrderBy(query, permanent);
        return this;
    }

    public IReadRepository<TId, TEntity> SetOrderBy(
        OrderBy orderBy,
        bool permanent,
        Expression<Func<TEntity, object>> expr
    )
    {
        base.SetOrderBy(GetSortDirection(orderBy), permanent, expr);
        return this;
    }

    public IReadRepository<TId, TEntity> SetOrderBy(
        OrderBy orderBy,
        Expression<Func<TEntity, object>> expr
    )
    {
        base.SetOrderBy(GetSortDirection(orderBy), expr);
        return this;
    }

    public IReadRepository<TId, TEntity> SetOrderBy<T>(
        OrderBy orderBy,
        Expression<Func<T, object>> expr
    )
    {
        base.SetOrderBy(GetSortDirection(orderBy), expr);
        return this;
    }

    public IReadRepository<TId, TEntity> SetOrderBy<T>(
        OrderBy orderBy,
        bool permanent,
        Expression<Func<T, object>> expr
    )
    {
        base.SetOrderBy(GetSortDirection(orderBy), permanent, expr);
        return this;
    }

    public new IReadRepository<TId, TEntity> SetGroupBy()
    {
        base.SetGroupBy();
        return this;
    }

    public new IReadRepository<TId, TEntity> SetGroupBy(
        bool permanent,
        Expression<Func<TEntity, object>> expr
    )
    {
        base.SetGroupBy(permanent, expr);
        return this;
    }

    public new IReadRepository<TId, TEntity> SetGroupBy<T>(
        bool permanent,
        Expression<Func<T, object>> expr
    )
    {
        base.SetGroupBy(permanent, expr);
        return this;
    }

    public new IReadRepository<TId, TEntity> SetOrderBy<T>(Expression<Func<T, object>> expr)
    {
        base.SetOrderBy(GetSortDirection(OrderBy.Ascending), expr);
        return this;
    }

    public new IReadRepository<TId, TEntity> SetGroupBy(Expression<Func<TEntity, object>> expr)
    {
        base.SetGroupBy(expr);
        return this;
    }

    public new IReadRepository<TId, TEntity> SetLimit()
    {
        base.SetLimit();
        return this;
    }

    public new IReadRepository<TId, TEntity> SetLimit(uint limit, uint? offset, bool permanent)
    {
        base.SetLimit(limit, offset, permanent);
        return this;
    }

    public new IReadRepository<TId, TEntity> SetLimit(uint limit, uint offset)
    {
        base.SetLimit(limit, offset);
        return this;
    }

    public new IReadRepository<TId, TEntity> SetLimit(uint limit)
    {
        base.SetLimit(limit);
        return this;
    }

    private static OrderInfo.SortDirection GetSortDirection(OrderBy orderBy) =>
        orderBy == OrderBy.Ascending ? OrderInfo.SortDirection.ASC : OrderInfo.SortDirection.DESC;
}
