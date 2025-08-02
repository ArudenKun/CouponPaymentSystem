using System.Linq.Expressions;
using System.Reflection;
using Abp.Collections.Extensions;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Uow;
using Abp.Events.Bus;
using Abp.Events.Bus.Entities;
using Abp.Extensions;
using Abp.Linq2Db.Common;
using Abp.Linq2Db.Configuration;
using Abp.Linq2Db.Utilities;
using Abp.Runtime.Session;
using Abp.Timing;
using Castle.Core.Logging;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.Mapping;

namespace Abp.Linq2Db;

public abstract class AbpDbContext
    : DataConnection,
        ITransientDependency,
        IShouldInitializeDbContext
{
    /// <summary>
    /// Used to get current session values.
    /// </summary>
    public IAbpSession AbpSession { get; set; }

    /// <summary>
    /// Used to trigger entity change events.
    /// </summary>
    public IEntityChangeEventHelper EntityChangeEventHelper { get; set; }

    /// <summary>
    /// Reference to the logger.
    /// </summary>
    public ILogger Logger { get; set; }

    /// <summary>
    /// Reference to the event bus.
    /// </summary>
    public IEventBus EventBus { get; set; }

    /// <summary>
    /// Reference to GUID generator.
    /// </summary>
    public IGuidGenerator GuidGenerator { get; set; }

    /// <summary>
    /// Reference to the current UOW provider.
    /// </summary>
    public ICurrentUnitOfWorkProvider CurrentUnitOfWorkProvider { get; set; }

    /// <summary>
    /// Reference to multi tenancy configuration.
    /// </summary>
    public IMultiTenancyConfig MultiTenancyConfig { get; set; }

    /// <summary>
    /// Reference to the ABP entity configuration.
    /// </summary>
    public IAbpLinq2DbConfiguration AbpLinq2DbConfiguration { get; set; }

    /// <summary>
    /// Can be used to suppress automatically setting TenantId on SaveChanges.
    /// Default: false.
    /// </summary>
    public virtual bool SuppressAutoSetTenantId { get; set; }

    public virtual int? CurrentTenantId => GetCurrentTenantIdOrNull();

    public virtual bool IsSoftDeleteFilterEnabled =>
        CurrentUnitOfWorkProvider?.Current?.IsFilterEnabled(AbpDataFilters.SoftDelete) == true;

    public virtual bool IsMayHaveTenantFilterEnabled =>
        CurrentUnitOfWorkProvider.Current?.IsFilterEnabled(AbpDataFilters.MayHaveTenant) == true;

    public virtual bool IsMustHaveTenantFilterEnabled =>
        CurrentTenantId != null
        && CurrentUnitOfWorkProvider.Current?.IsFilterEnabled(AbpDataFilters.MustHaveTenant)
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

    protected DataOptions DataOptions { get; }

    /// <summary>
    /// Constructor.
    /// </summary>
    protected AbpDbContext(DataOptions options)
        : base(options)
    {
        DataOptions = options;
        InitializeDbContext();
    }

    private void InitializeDbContext()
    {
        SetNullsForInjectedProperties();
    }

    private void SetNullsForInjectedProperties()
    {
        Logger = NullLogger.Instance;
        AbpSession = NullAbpSession.Instance;
        EntityChangeEventHelper = NullEntityChangeEventHelper.Instance;
        GuidGenerator = SequentialGuidGenerator.Instance;
        EventBus = NullEventBus.Instance;
        AbpLinq2DbConfiguration = NullAbpLinq2DbConfiguration.Instance;
    }

    private void OnModelCreating(FluentMappingBuilder modelBuilder, IList<Type> entityTypes)
    {
        foreach (var entityType in entityTypes)
        {
            ConfigureGlobalFiltersMethodInfo
                .MakeGenericMethod(entityType)
                .Invoke(this, [modelBuilder, entityType]);

            ConfigureGlobalValueConverterMethodInfo
                .MakeGenericMethod(entityType)
                .Invoke(this, [modelBuilder, entityType]);
        }
    }

    protected void ConfigureGlobalFilters<TEntity>(
        EntityMappingBuilder<TEntity> modelBuilder,
        Type entityType
    )
        where TEntity : class
    {
        if (entityType.BaseType != null || !ShouldFilterEntity<TEntity>(entityType))
            return;

        var filterExpression = CreateFilterExpression<TEntity>();
        if (filterExpression != null)
        {
            modelBuilder.Entity<TEntity>().HasQueryFilter(filterExpression);
        }
    }

    protected virtual bool ShouldFilterEntity<TEntity>(Type entityType)
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

    protected virtual Expression<Func<TEntity, bool>>? CreateFilterExpression<TEntity>()
        where TEntity : class
    {
        Expression<Func<TEntity, bool>>? expression = null;

        if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
        {
            Expression<Func<TEntity, bool>> softDeleteFilter = e =>
                !IsSoftDeleteFilterEnabled || !((ISoftDelete)e).IsDeleted;
            expression =
                expression == null
                    ? softDeleteFilter
                    : CombineExpressions(expression, softDeleteFilter);
        }

        if (typeof(IMayHaveTenant).IsAssignableFrom(typeof(TEntity)))
        {
            Expression<Func<TEntity, bool>> mayHaveTenantFilter = e =>
                !IsMayHaveTenantFilterEnabled || ((IMayHaveTenant)e).TenantId == CurrentTenantId;
            expression =
                expression == null
                    ? mayHaveTenantFilter
                    : CombineExpressions(expression, mayHaveTenantFilter);
        }

        if (typeof(IMustHaveTenant).IsAssignableFrom(typeof(TEntity)))
        {
            Expression<Func<TEntity, bool>> mustHaveTenantFilter = e =>
                !IsMustHaveTenantFilterEnabled || ((IMustHaveTenant)e).TenantId == CurrentTenantId;

            expression =
                expression == null
                    ? mustHaveTenantFilter
                    : CombineExpressions(expression, mustHaveTenantFilter);
        }

        return expression;
    }

    public virtual string GetCompiledQueryCacheKey()
    {
        return $"{CurrentTenantId?.ToString() ?? "Null"}:{IsSoftDeleteFilterEnabled}:{IsMayHaveTenantFilterEnabled}:{IsMustHaveTenantFilterEnabled}";
    }

    protected void ConfigureGlobalValueConverter<TEntity>(
        ModelBuilder modelBuilder,
        IMutableEntityType entityType
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
                modelBuilder
                    .Entity<TEntity>()
                    .Property(property.Name)
                    .HasConversion(dateTimeValueConverter);
            });
        }
    }

    public override int SaveChanges()
    {
        try
        {
            var changeReport = ApplyAbpConcepts();
            var result = base.SaveChanges();
            EntityChangeEventHelper.TriggerEvents(changeReport);
            return result;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new AbpDbConcurrencyException(ex.Message, ex);
        }
    }

    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default(CancellationToken)
    )
    {
        try
        {
            var changeReport = ApplyAbpConcepts();
            var result = await base.SaveChangesAsync(cancellationToken);
            await EntityChangeEventHelper.TriggerEventsAsync(changeReport);
            return result;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new AbpDbConcurrencyException(ex.Message, ex);
        }
    }

    public virtual void Initialize(AbpLinq2DbContextInitializationContext initializationContext)
    {
        var uowOptions = initializationContext.UnitOfWork.Options;
        if (
            uowOptions.Timeout.HasValue
            && Database.IsRelational()
            && !Database.GetCommandTimeout().HasValue
        )
        {
            Database.SetCommandTimeout(uowOptions.Timeout.Value.TotalSeconds.To<int>());
        }

        ChangeTracker.CascadeDeleteTiming = CascadeTiming.OnSaveChanges;
    }

    protected virtual EntityChangeReport ApplyAbpConcepts()
    {
        var changeReport = new EntityChangeReport();

        var userId = GetAuditUserId();

        foreach (var entry in ChangeTracker.Entries().ToList())
        {
            if (entry.State != EntityState.Modified && entry.CheckOwnedEntityChange())
            {
                Entry(entry.Entity).State = EntityState.Modified;
            }

            ApplyAbpConcepts(entry, userId, changeReport);
        }

        return changeReport;
    }

    protected virtual void ApplyAbpConcepts(
        EntityEntry entry,
        long? userId,
        EntityChangeReport changeReport
    )
    {
        switch (entry.State)
        {
            case EntityState.Added:
                ApplyAbpConceptsForAddedEntity(entry, userId, changeReport);
                break;
            case EntityState.Modified:
                ApplyAbpConceptsForModifiedEntity(entry, userId, changeReport);
                break;
            case EntityState.Deleted:
                ApplyAbpConceptsForDeletedEntity(entry, userId, changeReport);
                break;
        }

        AddDomainEvents(changeReport.DomainEvents, entry.Entity);
    }

    protected virtual void ApplyAbpConceptsForAddedEntity(
        EntityEntry entry,
        long? userId,
        EntityChangeReport changeReport
    )
    {
        CheckAndSetId(entry);
        CheckAndSetMustHaveTenantIdProperty(entry.Entity);
        CheckAndSetMayHaveTenantIdProperty(entry.Entity);
        SetCreationAuditProperties(entry.Entity, userId);
        changeReport.ChangedEntities.Add(
            new EntityChangeEntry(entry.Entity, EntityChangeType.Created)
        );
    }

    protected virtual void ApplyAbpConceptsForModifiedEntity(
        EntityEntry entry,
        long? userId,
        EntityChangeReport changeReport
    )
    {
        SetModificationAuditProperties(entry.Entity, userId);
        if (entry.Entity is ISoftDelete && entry.Entity.As<ISoftDelete>().IsDeleted)
        {
            SetDeletionAuditProperties(entry.Entity, userId);
            changeReport.ChangedEntities.Add(
                new EntityChangeEntry(entry.Entity, EntityChangeType.Deleted)
            );
        }
        else
        {
            changeReport.ChangedEntities.Add(
                new EntityChangeEntry(entry.Entity, EntityChangeType.Updated)
            );
        }
    }

    protected virtual void ApplyAbpConceptsForDeletedEntity(
        EntityEntry entry,
        long? userId,
        EntityChangeReport changeReport
    )
    {
        if (IsHardDeleteEntity(entry))
        {
            changeReport.ChangedEntities.Add(
                new EntityChangeEntry(entry.Entity, EntityChangeType.Deleted)
            );
            return;
        }

        CancelDeletionForSoftDelete(entry);
        SetDeletionAuditProperties(entry.Entity, userId);
        changeReport.ChangedEntities.Add(
            new EntityChangeEntry(entry.Entity, EntityChangeType.Deleted)
        );
    }

    protected virtual bool IsHardDeleteEntity(EntityEntry entry)
    {
        if (!EntityHelper.IsEntity(entry.Entity.GetType()))
        {
            return false;
        }

        if (CurrentUnitOfWorkProvider?.Current?.Items == null)
        {
            return false;
        }

        if (
            !CurrentUnitOfWorkProvider.Current.Items.ContainsKey(
                UnitOfWorkExtensionDataTypes.HardDelete
            )
        )
        {
            return false;
        }

        var hardDeleteItems = CurrentUnitOfWorkProvider.Current.Items[
            UnitOfWorkExtensionDataTypes.HardDelete
        ];
        if (!(hardDeleteItems is HashSet<string> objects))
        {
            return false;
        }

        var currentTenantId = GetCurrentTenantIdOrNull();
        var hardDeleteKey = EntityHelper.GetHardDeleteKey(entry.Entity, currentTenantId);
        return objects.Contains(hardDeleteKey);
    }

    protected virtual void AddDomainEvents(List<DomainEventEntry> domainEvents, object entityAsObj)
    {
        var generatesDomainEventsEntity = entityAsObj as IGeneratesDomainEvents;
        if (generatesDomainEventsEntity == null)
        {
            return;
        }

        if (generatesDomainEventsEntity.DomainEvents.IsNullOrEmpty())
        {
            return;
        }

        domainEvents.AddRange(
            generatesDomainEventsEntity.DomainEvents.Select(eventData => new DomainEventEntry(
                entityAsObj,
                eventData
            ))
        );
        generatesDomainEventsEntity.DomainEvents.Clear();
    }

    protected virtual void CheckAndSetId(EntityEntry entry)
    {
        //Set GUID Ids
        var entity = entry.Entity as IEntity<Guid>;
        if (entity != null && entity.Id == Guid.Empty)
        {
            var idPropertyEntry = entry.Property("Id");

            if (
                idPropertyEntry != null
                && idPropertyEntry.Metadata.ValueGenerated == ValueGenerated.Never
            )
            {
                entity.Id = GuidGenerator.Create();
            }
        }
    }

    protected virtual void CheckAndSetMustHaveTenantIdProperty(object entityAsObj)
    {
        if (SuppressAutoSetTenantId)
        {
            return;
        }

        //Only set IMustHaveTenant entities
        if (!(entityAsObj is IMustHaveTenant))
        {
            return;
        }

        var entity = entityAsObj.As<IMustHaveTenant>();

        //Don't set if it's already set
        if (entity.TenantId != 0)
        {
            return;
        }

        var currentTenantId = GetCurrentTenantIdOrNull();

        if (currentTenantId != null)
        {
            entity.TenantId = currentTenantId.Value;
        }
        else
        {
            throw new AbpException("Can not set TenantId to 0 for IMustHaveTenant entities!");
        }
    }

    protected virtual void CheckAndSetMayHaveTenantIdProperty(object entityAsObj)
    {
        if (SuppressAutoSetTenantId)
        {
            return;
        }

        //Only works for single tenant applications
        if (MultiTenancyConfig?.IsEnabled ?? false)
        {
            return;
        }

        //Only set IMayHaveTenant entities
        if (!(entityAsObj is IMayHaveTenant))
        {
            return;
        }

        var entity = entityAsObj.As<IMayHaveTenant>();

        //Don't set if it's already set
        if (entity.TenantId != null)
        {
            return;
        }

        entity.TenantId = GetCurrentTenantIdOrNull();
    }

    protected virtual void SetCreationAuditProperties(object entityAsObj, long? userId)
    {
        EntityAuditingHelper.SetCreationAuditProperties(
            MultiTenancyConfig,
            entityAsObj,
            AbpSession.TenantId,
            userId,
            CurrentUnitOfWorkProvider?.Current?.AuditFieldConfiguration
        );
    }

    protected virtual void SetModificationAuditProperties(object entityAsObj, long? userId)
    {
        EntityAuditingHelper.SetModificationAuditProperties(
            MultiTenancyConfig,
            entityAsObj,
            AbpSession.TenantId,
            userId,
            CurrentUnitOfWorkProvider?.Current?.AuditFieldConfiguration
        );
    }

    protected virtual void CancelDeletionForSoftDelete(EntityEntry entry)
    {
        if (!(entry.Entity is ISoftDelete))
        {
            return;
        }

        entry.Reload();
        entry.State = EntityState.Modified;
        entry.Entity.As<ISoftDelete>().IsDeleted = true;
    }

    protected virtual void SetDeletionAuditProperties(object entityAsObj, long? userId)
    {
        EntityAuditingHelper.SetDeletionAuditProperties(
            MultiTenancyConfig,
            entityAsObj,
            AbpSession.TenantId,
            userId,
            CurrentUnitOfWorkProvider?.Current?.AuditFieldConfiguration
        );
    }

    protected virtual long? GetAuditUserId()
    {
        if (
            AbpSession.UserId.HasValue
            && CurrentUnitOfWorkProvider != null
            && CurrentUnitOfWorkProvider.Current != null
            && CurrentUnitOfWorkProvider.Current.GetTenantId() == AbpSession.TenantId
        )
        {
            return AbpSession.UserId;
        }

        return null;
    }

    protected virtual int? GetCurrentTenantIdOrNull()
    {
        if (CurrentUnitOfWorkProvider != null && CurrentUnitOfWorkProvider.Current != null)
        {
            return CurrentUnitOfWorkProvider.Current.GetTenantId();
        }

        return AbpSession.TenantId;
    }

    protected virtual Expression<Func<T, bool>> CombineExpressions<T>(
        Expression<Func<T, bool>> expression1,
        Expression<Func<T, bool>> expression2
    )
    {
        return ExpressionCombiner.Combine(expression1, expression2);
    }
}
