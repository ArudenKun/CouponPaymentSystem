using System.Linq.Expressions;
using Abp.Dependency;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Linq.Expressions;
using Abp.Reflection;
using Abp.Runtime.Session;
using NHibernate;
using NHibernate.Linq;

namespace Abp.NHibernate.Repositories;

public static class NhRepositoryExtensions
{
    public static ISession GetSession<TEntity, TPrimaryKey>(
        this IRepository<TEntity, TPrimaryKey> repository
    )
        where TEntity : class, IEntity<TPrimaryKey>
    {
        if (ProxyHelper.UnProxy(repository) is IRepositoryWithSession repositoryWithDbContext)
        {
            return repositoryWithDbContext.GetSession();
        }

        throw new ArgumentException(
            "Given repository does not implement IRepositoryWithDbContext",
            nameof(repository)
        );
    }

    public static async Task BatchInsertAsync<TEntity, TPrimaryKey>(
        this IRepository<TEntity, TPrimaryKey> repository,
        IEnumerable<TEntity> entities,
        int batchSize = 100
    )
        where TEntity : Entity<TPrimaryKey>
    {
        Check.NotNull(repository, nameof(repository));

        var session = repository.GetSession();
        var entitiesArray = entities.ToArray();
        var totalCount = entitiesArray.Length;
        var processed = 0;

        while (processed < totalCount)
        {
            var currentBatchSize = Math.Min(batchSize, totalCount - processed);
            for (var i = processed; i < processed + currentBatchSize; i++)
            {
                await repository.InsertAsync(entitiesArray[i]);
            }

            await session.FlushAsync();
            session.Clear();
            processed += currentBatchSize;
        }
    }

    /// <summary>
    /// Deletes all matching entities permanently for given predicate
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TPrimaryKey">Primary key type</typeparam>
    /// <param name="repository">Repository</param>
    /// <param name="predicate">Predicate to filter entities</param>
    /// <returns></returns>
    public static async Task<int> BatchDeleteAsync<TEntity, TPrimaryKey>(
        this IRepository<TEntity, TPrimaryKey> repository,
        Expression<Func<TEntity, bool>> predicate
    )
        where TEntity : Entity<TPrimaryKey>
    {
        Check.NotNull(repository, nameof(repository));
        Check.NotNull(predicate, nameof(predicate));

        return await repository.Query(async query =>
        {
            var abpFilterExpression = GetFilterExpressionOrNull<TEntity, TPrimaryKey>(
                repository.GetIocResolver()
            );
            var filterExpression = ExpressionCombiner.Combine(predicate, abpFilterExpression);

            return await query.Where(filterExpression).DeleteAsync();
        });
    }

    private static Expression<Func<TEntity, bool>>? GetFilterExpressionOrNull<TEntity, TPrimaryKey>(
        IIocResolver iocResolver
    )
        where TEntity : Entity<TPrimaryKey>
    {
        Expression<Func<TEntity, bool>>? expression = null;

        using var scope = iocResolver.CreateScope();
        var currentUnitOfWorkProvider = scope.Resolve<ICurrentUnitOfWorkProvider>();

        if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
        {
            var isSoftDeleteFilterEnabled =
                currentUnitOfWorkProvider.Current?.IsFilterEnabled(AbpDataFilters.SoftDelete)
                == true;
            if (isSoftDeleteFilterEnabled)
            {
                Expression<Func<TEntity, bool>> softDeleteFilter = e => !((ISoftDelete)e).IsDeleted;
                expression = softDeleteFilter;
            }
        }

        if (typeof(IMayHaveTenant).IsAssignableFrom(typeof(TEntity)))
        {
            var isMayHaveTenantFilterEnabled =
                currentUnitOfWorkProvider.Current?.IsFilterEnabled(AbpDataFilters.MayHaveTenant)
                == true;
            var currentTenantId = GetCurrentTenantIdOrNull(iocResolver);

            if (isMayHaveTenantFilterEnabled)
            {
                Expression<Func<TEntity, bool>> mayHaveTenantFilter = e =>
                    ((IMayHaveTenant)e).TenantId == currentTenantId;
                expression =
                    expression == null
                        ? mayHaveTenantFilter
                        : ExpressionCombiner.Combine(expression, mayHaveTenantFilter);
            }
        }

        if (typeof(IMustHaveTenant).IsAssignableFrom(typeof(TEntity)))
        {
            var isMustHaveTenantFilterEnabled =
                currentUnitOfWorkProvider.Current?.IsFilterEnabled(AbpDataFilters.MustHaveTenant)
                == true;
            var currentTenantId = GetCurrentTenantIdOrNull(iocResolver);

            if (isMustHaveTenantFilterEnabled)
            {
                Expression<Func<TEntity, bool>> mustHaveTenantFilter = e =>
                    ((IMustHaveTenant)e).TenantId == currentTenantId;
                expression =
                    expression == null
                        ? mustHaveTenantFilter
                        : ExpressionCombiner.Combine(expression, mustHaveTenantFilter);
            }
        }

        return expression;
    }

    private static int? GetCurrentTenantIdOrNull(IIocResolver iocResolver)
    {
        using (var scope = iocResolver.CreateScope())
        {
            var currentUnitOfWorkProvider = scope.Resolve<ICurrentUnitOfWorkProvider>();

            if (currentUnitOfWorkProvider?.Current != null)
            {
                return currentUnitOfWorkProvider.Current.GetTenantId();
            }

            return iocResolver.Resolve<IAbpSession>().TenantId;
        }
    }
}
