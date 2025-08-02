using System.Linq.Expressions;
using System.Reflection;
using Abp.Dependency;
using Abp.Domain.Entities;
using Abp.Domain.Uow;
using Abp.Linq2Db.Utilities;
using Abp.Runtime.Session;
using Abp.Timing;
using Castle.Core.Logging;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.Mapping;

namespace Abp.Linq2Db;

public abstract class AbpDbContext : DataConnection, ITransientDependency
{
    /// <summary>
    /// Used to get current session values.
    /// </summary>
    public IAbpSession AbpSession { get; private set; }

    /// <summary>
    /// Reference to the logger.
    /// </summary>
    public ILogger Logger { get; private set; }

    /// <summary>
    /// Reference to GUID generator.
    /// </summary>
    public IGuidGenerator GuidGenerator { get; }

    /// <summary>
    /// Reference to the current UOW provider.
    /// </summary>
    public ICurrentUnitOfWorkProvider? CurrentUnitOfWorkProvider { get; private set; }

    /// <summary>
    /// Can be used to suppress automatically setting TenantId on SaveChanges.
    /// Default: false.
    /// </summary>
    public virtual bool SuppressAutoSetTenantId { get; set; }

    public virtual int? CurrentTenantId => GetCurrentTenantIdOrNull();

    public virtual bool IsSoftDeleteFilterEnabled =>
        CurrentUnitOfWorkProvider?.Current?.IsFilterEnabled(AbpDataFilters.SoftDelete) == true;

    public virtual bool IsMayHaveTenantFilterEnabled =>
        CurrentUnitOfWorkProvider?.Current?.IsFilterEnabled(AbpDataFilters.MayHaveTenant) == true;

    public virtual bool IsMustHaveTenantFilterEnabled =>
        CurrentTenantId != null
        && CurrentUnitOfWorkProvider?.Current?.IsFilterEnabled(AbpDataFilters.MustHaveTenant)
            == true;

    private static readonly MethodInfo? ConfigureGlobalFiltersMethodInfo =
        typeof(AbpDbContext).GetMethod(
            nameof(ConfigureGlobalFilters),
            BindingFlags.Instance | BindingFlags.NonPublic
        );

    private static readonly MethodInfo? ConfigureGlobalValueConverterMethodInfo =
        typeof(AbpDbContext).GetMethod(
            nameof(ConfigureGlobalValueConverter),
            BindingFlags.Instance | BindingFlags.NonPublic
        );

    protected AbpDbContext(DataOptions options)
        : base(options)
    {
        AbpSession = NullAbpSession.Instance;
        Logger = NullLogger.Instance;
        GuidGenerator = SequentialGuidGenerator.Instance;

        var fluentMappingBuilder = new FluentMappingBuilder();
        Initialize(fluentMappingBuilder);
        AddMappingSchema(fluentMappingBuilder.MappingSchema);
    }

    private void Initialize(FluentMappingBuilder fluentMappingBuilder)
    {
        var entityTypes = MappingSchema.GetDefinedTypes();
        foreach (var entityType in entityTypes)
        {
            ConfigureGlobalFiltersMethodInfo
                ?.MakeGenericMethod(entityType)
                .Invoke(this, [fluentMappingBuilder, entityType]);

            ConfigureGlobalValueConverterMethodInfo
                .MakeGenericMethod(entityType)
                .Invoke(this, [fluentMappingBuilder, entityType]);
        }
    }

    protected void ConfigureGlobalFilters<TEntity>(
        FluentMappingBuilder fluentMappingBuilder,
        Type entityType
    )
        where TEntity : class
    {
        var mappingBuilder = fluentMappingBuilder.Entity<TEntity>();
        if (entityType.BaseType != null || !ShouldFilterEntity<TEntity>())
            return;
        var filterExpression = CreateFilterExpression(mappingBuilder);
        if (filterExpression != null)
        {
            mappingBuilder.HasQueryFilter(filterExpression);
        }
    }

    protected virtual bool ShouldFilterEntity<TEntity>()
        where TEntity : class
    {
        if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
        {
            return true;
        }

        if (typeof(IMayHaveTenant).IsAssignableFrom(typeof(TEntity)))
        {
            return true;
        }

        if (typeof(IMustHaveTenant).IsAssignableFrom(typeof(TEntity)))
        {
            return true;
        }

        return false;
    }

    protected virtual Expression<Func<TEntity, bool>>? CreateFilterExpression<TEntity>(
        EntityMappingBuilder<TEntity> mappingBuilder
    )
        where TEntity : class
    {
        Expression<Func<TEntity, bool>>? expression = null;

        if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)) && IsSoftDeleteFilterEnabled)
        {
            Expression<Func<TEntity, bool>> softDeleteFilter = e =>
                SoftDeleteFilter(((ISoftDelete)e).IsDeleted);
            expression =
                expression == null
                    ? softDeleteFilter
                    : CombineExpressions(expression, softDeleteFilter);
        }

        if (
            typeof(IMayHaveTenant).IsAssignableFrom(typeof(TEntity)) && IsMayHaveTenantFilterEnabled
        )
        {
            Expression<Func<TEntity, bool>> mayHaveTenantFilter = e =>
                MayHaveTenantFilter(((IMayHaveTenant)e).TenantId, CurrentTenantId);

            expression =
                expression == null
                    ? mayHaveTenantFilter
                    : CombineExpressions(expression, mayHaveTenantFilter);
        }

        if (
            typeof(IMustHaveTenant).IsAssignableFrom(typeof(TEntity))
            && IsMustHaveTenantFilterEnabled
        )
        {
            Expression<Func<TEntity, bool>> mustHaveTenantFilter = e =>
                MustHaveTenantFilter(((IMustHaveTenant)e).TenantId, CurrentTenantId);
            expression =
                expression == null
                    ? mustHaveTenantFilter
                    : CombineExpressions(expression, mustHaveTenantFilter);
        }

        return expression;
    }

    public static bool SoftDeleteFilter(bool isDeleted) => !isDeleted;

    public static bool MustHaveTenantFilter(int tenantId, int? currentTenantId) =>
        tenantId == currentTenantId;

    public static bool MayHaveTenantFilter(int? tenantId, int? currentTenantId) =>
        tenantId == currentTenantId;

    protected void ConfigureGlobalValueConverter<TEntity>(
        EntityMappingBuilder<TEntity> mappingBuilder,
        Type entityType
    )
        where TEntity : class
    {
        if (
            entityType.BaseType == null
            && !typeof(TEntity).IsDefined(typeof(DisableDateTimeNormalizationAttribute), true)
            && !typeof(TEntity).IsDefined(typeof(OwnedAttribute), true)
            && !entityType.IsOwned()
        )
        {
            var dateTimeValueConverter = new AbpDateTimeValueConverter();
            var dateTimePropertyInfos = DateTimePropertyInfoHelper.GetDatePropertyInfos(
                typeof(TEntity)
            );
            dateTimePropertyInfos.DateTimePropertyInfos.ForEach(property =>
            {
                mappingBuilder.Property(property.Name).HasConversion();
            });
        }
    }

    protected virtual int? GetCurrentTenantIdOrNull() =>
        CurrentUnitOfWorkProvider is { Current: not null }
            ? CurrentUnitOfWorkProvider.Current.GetTenantId()
            : AbpSession.TenantId;

    protected virtual Expression<Func<T, bool>> CombineExpressions<T>(
        Expression<Func<T, bool>> expression1,
        Expression<Func<T, bool>> expression2
    )
    {
        return ExpressionCombiner.Combine(expression1, expression2);
    }
}
