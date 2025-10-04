using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using Abp.Domain.Entities;
using Abp.Domain.Uow;
using Abp.LinqToDb.Utilities;
using Abp.Runtime.Session;
using LinqToDB;
using LinqToDB.Async;
using LinqToDB.Data;
using LinqToDB.Mapping;

namespace Abp.LinqToDb.Repositories;

[SuppressMessage("ReSharper", "StaticMemberInGenericType")]
internal abstract class AbpLinqToDbRepositoryBase<TEntity, TPrimaryKey>
    : ILinqToDbRepository<TEntity, TPrimaryKey>
    where TEntity : class, IEntity<TPrimaryKey>
{
    private static MappingSchema? _mappingSchema;

    protected AbpLinqToDbRepositoryBase(DataOptions dataOptions)
    {
        Options = dataOptions.UseAdditionalMappingSchema(InitializedMappingSchema);
    }

    /// <summary>
    /// Used to get current session values.
    /// </summary>
    public IAbpSession AbpSession { get; set; } = NullAbpSession.Instance;

    /// <summary>
    /// Reference to the current UOW provider.
    /// </summary>
    public ICurrentUnitOfWorkProvider? CurrentUnitOfWorkProvider { get; set; }

    public DataOptions Options { get; }

    protected abstract DataConnection Connection { get; }

    protected virtual ITable<TEntity> Table => Connection.GetTable<TEntity>();

    private MappingSchema InitializedMappingSchema => _mappingSchema ??= InitMappingSchema();

    private static readonly MethodInfo? ConfigureGlobalFiltersMethodInfo =
        typeof(AbpLinqToDbRepositoryBase<TEntity, TPrimaryKey>).GetMethod(
            nameof(ConfigureGlobalFilters),
            BindingFlags.Instance | BindingFlags.NonPublic
        );

    protected static Expression<Func<TEntity, bool>> CreateEqualityExpressionForId(TPrimaryKey id)
    {
        var lambdaParam = Expression.Parameter(typeof(TEntity));
        var leftExpression = Expression.PropertyOrField(lambdaParam, "Id");
        var idValue = Convert.ChangeType(id, typeof(TPrimaryKey));
        Expression<Func<object>> closure = () => idValue;
        var rightExpression = Expression.Convert(closure.Body, leftExpression.Type);
        var lambdaBody = Expression.Equal(leftExpression, rightExpression);
        return Expression.Lambda<Func<TEntity, bool>>(lambdaBody, lambdaParam);
    }

    private MappingSchema InitMappingSchema()
    {
        var builder = new FluentMappingBuilder();
        foreach (var type in Options.ConnectionOptions.MappingSchema?.GetDefinedTypes() ?? [])
        {
            ConfigureGlobalFiltersMethodInfo?.MakeGenericMethod(type).Invoke(this, [builder, type]);
        }

        builder.Build();
        return builder.MappingSchema;
    }

    private void ConfigureGlobalFilters<TEntityFilter>(
        FluentMappingBuilder builder,
        Type entityType
    )
        where TEntityFilter : class
    {
        var entityBuilder = builder.Entity<TEntityFilter>();
        if (entityType.BaseType != null || !ShouldFilterEntity<TEntityFilter>())
            return;
        var filterExpression = CreateFilterExpression<TEntityFilter>();
        if (filterExpression != null)
        {
            entityBuilder.HasQueryFilter(filterExpression);
        }
    }

    protected virtual bool ShouldFilterEntity<TEntityFilter>()
        where TEntityFilter : class
    {
        if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntityFilter)))
        {
            return true;
        }

        return typeof(IMayHaveTenant).IsAssignableFrom(typeof(TEntityFilter))
            || typeof(IMustHaveTenant).IsAssignableFrom(typeof(TEntityFilter));
    }

    protected virtual Expression<
        Func<TEntityFilter, IDataContext, bool>
    >? CreateFilterExpression<TEntityFilter>()
        where TEntityFilter : class
    {
        Expression<Func<TEntityFilter, IDataContext, bool>>? expression = null;

        if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntityFilter)))
        {
            Expression<Func<TEntityFilter, IDataContext, bool>> softDeleteFilter = (e, _) =>
                !IsSoftDeleteFilterEnabled || !((ISoftDelete)e).IsDeleted;

            expression =
                expression == null
                    ? softDeleteFilter
                    : CombineExpressions(expression, softDeleteFilter);
        }

        if (typeof(IMayHaveTenant).IsAssignableFrom(typeof(TEntityFilter)))
        {
            Expression<Func<TEntityFilter, IDataContext, bool>> mayHaveTenantFilter = (e, _) =>
                !IsMayHaveTenantFilterEnabled || ((IMayHaveTenant)e).TenantId == CurrentTenantId;
            expression =
                expression == null
                    ? mayHaveTenantFilter
                    : CombineExpressions(expression, mayHaveTenantFilter);
        }

        if (typeof(IMustHaveTenant).IsAssignableFrom(typeof(TEntityFilter)))
        {
            Expression<Func<TEntityFilter, IDataContext, bool>> mustHaveTenantFilter = (e, _) =>
                !IsMustHaveTenantFilterEnabled || ((IMustHaveTenant)e).TenantId == CurrentTenantId;
            expression =
                expression == null
                    ? mustHaveTenantFilter
                    : CombineExpressions(expression, mustHaveTenantFilter);
        }

        return expression;
    }

    public virtual int? CurrentTenantId => GetCurrentTenantIdOrNull();

    public virtual bool IsSoftDeleteFilterEnabled =>
        CurrentUnitOfWorkProvider?.Current?.IsFilterEnabled(AbpDataFilters.SoftDelete) == true;

    public virtual bool IsMayHaveTenantFilterEnabled =>
        CurrentUnitOfWorkProvider?.Current?.IsFilterEnabled(AbpDataFilters.MayHaveTenant) == true;

    public virtual bool IsMustHaveTenantFilterEnabled =>
        CurrentTenantId != null
        && CurrentUnitOfWorkProvider?.Current?.IsFilterEnabled(AbpDataFilters.MustHaveTenant)
            == true;

    protected virtual int? GetCurrentTenantIdOrNull() =>
        CurrentUnitOfWorkProvider is { Current: not null }
            ? CurrentUnitOfWorkProvider.Current.GetTenantId()
            : AbpSession.TenantId;

    protected virtual Expression<Func<T, IDataContext, bool>>? CombineExpressions<T>(
        Expression<Func<T, IDataContext, bool>> expression1,
        Expression<Func<T, IDataContext, bool>> expression2
    ) => ExpressionCombiner.Combine(expression1, expression2);

    public virtual IQueryable<TEntity> Query() => Table;

    public virtual TEntity Get(TPrimaryKey id)
    {
        var entity = FirstOrDefault(id);
        return entity ?? throw new EntityNotFoundException(typeof(TEntity), id);
    }

    public async Task<TEntity> GetAsync(
        TPrimaryKey id,
        CancellationToken cancellationToken = default
    )
    {
        var entity = await FirstOrDefaultAsync(id, cancellationToken);
        return entity ?? throw new EntityNotFoundException(typeof(TEntity), id);
    }

    public TEntity Single(TPrimaryKey id)
    {
        return Table.Single(CreateEqualityExpressionForId(id));
    }

    public TEntity Single(Func<TEntity, bool> predicate)
    {
        return Table.Single(predicate);
    }

    public TEntity Single(Expression<Func<TEntity, bool>> predicate)
    {
        return Table.Single(predicate);
    }

    public Task<TEntity> SingleAsync(TPrimaryKey id, CancellationToken cancellationToken = default)
    {
        return Table.SingleAsync(CreateEqualityExpressionForId(id), cancellationToken);
    }

    public Task<TEntity> SingleAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default
    )
    {
        return Table.SingleAsync(predicate, cancellationToken);
    }

    public TEntity? FirstOrDefault(TPrimaryKey id)
    {
        return Table.FirstOrDefault(CreateEqualityExpressionForId(id));
    }

    public TEntity? FirstOrDefault(Func<TEntity, bool> predicate)
    {
        return Table.FirstOrDefault(predicate);
    }

    public TEntity? FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
    {
        return Table.FirstOrDefault(predicate);
    }

    public Task<TEntity?> FirstOrDefaultAsync(
        TPrimaryKey id,
        CancellationToken cancellationToken = default
    )
    {
        return Table.FirstOrDefaultAsync(CreateEqualityExpressionForId(id), cancellationToken);
    }

    public Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default
    )
    {
        return Table.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public TEntity Insert(TEntity entity)
    {
        Connection.Insert(entity);
        return entity;
    }

    public async Task<TEntity> InsertAsync(
        TEntity entity,
        CancellationToken cancellationToken = default
    )
    {
        await Connection.InsertAsync(entity, token: cancellationToken);
        return entity;
    }

    public TPrimaryKey InsertAndGetId(TEntity entity)
    {
        return (TPrimaryKey)Connection.InsertWithIdentity(entity);
    }

    public async Task<TPrimaryKey> InsertAndGetIdAsync(
        TEntity entity,
        CancellationToken cancellationToken = default
    )
    {
        return (TPrimaryKey)
            await Connection.InsertWithIdentityAsync(entity, token: cancellationToken);
    }

    public TEntity InsertOrUpdate(TEntity entity)
    {
        Connection.InsertOrReplace(entity);
        return entity;
    }

    public async Task<TEntity> InsertOrUpdateAsync(
        TEntity entity,
        CancellationToken cancellationToken = default
    )
    {
        await Connection.InsertOrReplaceAsync(entity, token: cancellationToken);
        return entity;
    }

    public TPrimaryKey InsertOrUpdateAndGetId(TEntity entity)
    {
        Connection.InsertOrReplace(entity);
        return entity.Id;
    }

    public async Task<TPrimaryKey> InsertOrUpdateAndGetIdAsync(
        TEntity entity,
        CancellationToken cancellationToken = default
    )
    {
        await Connection.InsertOrReplaceAsync(entity, token: cancellationToken);
        return entity.Id;
    }

    public TEntity Update(TEntity entity)
    {
        Connection.Update(entity);
        return entity;
    }

    public async Task<TEntity> UpdateAsync(
        TEntity entity,
        CancellationToken cancellationToken = default
    )
    {
        await Connection.UpdateAsync(entity, token: cancellationToken);
        return entity;
    }

    public void Delete(TEntity entity)
    {
        Connection.Delete(entity);
    }

    public void Delete(TPrimaryKey id)
    {
        Table.Delete(CreateEqualityExpressionForId(id));
    }

    public void Delete(Expression<Func<TEntity, bool>> predicate)
    {
        Table.Delete(predicate);
    }

    public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        return Connection.DeleteAsync(entity, token: cancellationToken);
    }

    public Task DeleteAsync(TPrimaryKey id, CancellationToken cancellationToken = default)
    {
        return Table.DeleteAsync(CreateEqualityExpressionForId(id), token: cancellationToken);
    }

    public Task DeleteAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default
    )
    {
        return Table.DeleteAsync(predicate, token: cancellationToken);
    }

    public int Count()
    {
        return Table.Count();
    }

    public int Count(Func<TEntity, bool> predicate)
    {
        return Table.Count(predicate);
    }

    public int Count(Expression<Func<TEntity, bool>> predicate)
    {
        return Table.Count(predicate);
    }

    public Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return Table.CountAsync(cancellationToken);
    }

    public Task<int> CountAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default
    )
    {
        return Table.CountAsync(predicate, cancellationToken);
    }

    public long LongCount()
    {
        return Table.LongCount();
    }

    public long LongCount(Func<TEntity, bool> predicate)
    {
        return Table.LongCount(predicate);
    }

    public long LongCount(Expression<Func<TEntity, bool>> predicate)
    {
        return Table.LongCount(predicate);
    }

    public Task<long> LongCountAsync(CancellationToken cancellationToken = default)
    {
        return Table.LongCountAsync(cancellationToken);
    }

    public Task<long> LongCountAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default
    )
    {
        return Table.LongCountAsync(predicate, cancellationToken);
    }
}
