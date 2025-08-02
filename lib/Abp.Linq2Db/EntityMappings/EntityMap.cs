using System.Linq.Expressions;
using System.Reflection;
using Abp.Domain.Entities;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.Internal.DataProvider.Firebird;
using LinqToDB.Mapping;

namespace Abp.Linq2Db.EntityMappings;

public abstract class EntityMap<TEntity> : EntityMap<TEntity, int>
    where TEntity : IEntity<int>;

public abstract class EntityMap<TEntity, TPrimaryKey>
    where TEntity : IEntity<TPrimaryKey>
{
    private readonly FluentMappingBuilder _builder;
    private readonly EntityMappingBuilder<TEntity> _entityBuilder;

    protected EntityMap()
    {
        _builder = new FluentMappingBuilder();
        _entityBuilder = _builder.Entity<TEntity>();
    }

    protected abstract string TableName { get; }

    /// <summary>Adds mapping attribute to current entity.</summary>
    /// <param name="attribute">Mapping attribute to add.</param>
    /// <returns>Returns current fluent entity mapping builder.</returns>
    public void HasAttribute(MappingAttribute attribute) => _entityBuilder.HasAttribute(attribute);

    /// <summary>Adds mapping attribute to specified member.</summary>
    /// <param name="memberInfo">Target member.</param>
    /// <param name="attribute">Mapping attribute to add to specified member.</param>
    /// <returns>Returns current fluent entity mapping builder.</returns>
    public void HasAttribute(MemberInfo memberInfo, MappingAttribute attribute) =>
        _entityBuilder.HasAttribute(memberInfo, attribute);

    /// <summary>
    /// Adds mapping attribute to a member, specified using lambda expression.
    /// </summary>
    /// <param name="func">Target member, specified using lambda expression.</param>
    /// <param name="attribute">Mapping attribute to add to specified member.</param>
    /// <returns>Returns current fluent entity mapping builder.</returns>
    public void HasAttribute(LambdaExpression func, MappingAttribute attribute) =>
        _entityBuilder.HasAttribute(func, attribute);

    /// <summary>
    /// Adds mapping attribute to a member, specified using lambda expression.
    /// </summary>
    /// <param name="func">Target member, specified using lambda expression.</param>
    /// <param name="attribute">Mapping attribute to add to specified member.</param>
    /// <returns>Returns current fluent entity mapping builder.</returns>
    public void HasAttribute(Expression<Func<TEntity, object?>> func, MappingAttribute attribute) =>
        _entityBuilder.HasAttribute(func, attribute);

    /// <summary>Adds column mapping to current entity.</summary>
    /// <param name="func">Column mapping property or field getter expression.</param>
    /// <returns>Returns fluent property mapping builder.</returns>
    protected PropertyMappingBuilder<TEntity, TProperty> Property<TProperty>(
        Expression<Func<TEntity, TProperty>> func
    ) => new PropertyMappingBuilder<TEntity, TProperty>(_entityBuilder, func).IsColumn();

    /// <summary>Adds association mapping to current entity.</summary>
    /// <typeparam name="TProperty">Association member type.</typeparam>
    /// <typeparam name="TThisKey">This association side key type.</typeparam>
    /// <typeparam name="TOtherKey">Other association side key type.</typeparam>
    /// <param name="prop">Association member getter expression.</param>
    /// <param name="thisKey">This association key getter expression.</param>
    /// <param name="otherKey">Other association key getter expression.</param>
    /// <param name="canBeNull">Defines type of join. True - left join, False - inner join.</param>
    /// <returns>Returns fluent property mapping builder.</returns>
    public void Association<TProperty, TThisKey, TOtherKey>(
        Expression<Func<TEntity, TProperty>> prop,
        Expression<Func<TEntity, TThisKey>> thisKey,
        Expression<Func<TProperty, TOtherKey>> otherKey,
        bool? canBeNull = null
    ) => _entityBuilder.Association(prop, thisKey, otherKey, canBeNull);

    /// <summary>Adds association mapping to current entity.</summary>
    /// <typeparam name="TPropElement">Association member type.</typeparam>
    /// <typeparam name="TThisKey">This association side key type.</typeparam>
    /// <typeparam name="TOtherKey">Other association side key type.</typeparam>
    /// <param name="prop">Association member getter expression.</param>
    /// <param name="thisKey">This association key getter expression.</param>
    /// <param name="otherKey">Other association key getter expression.</param>
    /// <param name="canBeNull">Defines type of join. True - left join, False - inner join.</param>
    /// <returns>Returns fluent property mapping builder.</returns>
    public void Association<TPropElement, TThisKey, TOtherKey>(
        Expression<Func<TEntity, IEnumerable<TPropElement>>> prop,
        Expression<Func<TEntity, TThisKey>> thisKey,
        Expression<Func<TPropElement, TOtherKey>> otherKey,
        bool? canBeNull = null
    ) => _entityBuilder.Association(prop, thisKey, otherKey, canBeNull);

    /// <summary>
    /// Adds one-to-many association mapping to current entity.
    /// </summary>
    /// <typeparam name="TOther">Other association side type</typeparam>
    /// <param name="prop">Association member getter expression.</param>
    /// <param name="predicate">Predicate expression.</param>
    /// <param name="canBeNull">Defines type of join. True - left join, False - inner join.</param>
    /// <returns>Returns fluent property mapping builder.</returns>
    public void Association<TOther>(
        Expression<Func<TEntity, IEnumerable<TOther>>> prop,
        Expression<Func<TEntity, TOther, bool>> predicate,
        bool? canBeNull = null
    ) => _entityBuilder.Association(prop, predicate, canBeNull);

    /// <summary>
    /// Adds one-to-one association mapping to current entity.
    /// </summary>
    /// <typeparam name="TOther">Other association side type</typeparam>
    /// <param name="prop">Association member getter expression.</param>
    /// <param name="predicate">Predicate expression</param>
    /// <param name="canBeNull">Defines type of join. True - left join, False - inner join.</param>
    /// <returns>Returns fluent property mapping builder.</returns>
    public void Association<TOther>(
        Expression<Func<TEntity, TOther>> prop,
        Expression<Func<TEntity, TOther, bool>> predicate,
        bool? canBeNull = null
    ) => _entityBuilder.Association(prop, predicate, canBeNull);

    /// <summary>
    /// Adds one-to-many association mapping to current entity.
    /// </summary>
    /// <typeparam name="TOther">Other association side type</typeparam>
    /// <param name="prop">Association member getter expression.</param>
    /// <param name="queryExpression">Query expression.</param>
    /// <param name="canBeNull">Defines type of join. True - left join, False - inner join.</param>
    /// <returns>Returns fluent property mapping builder.</returns>
    public void Association<TOther>(
        Expression<Func<TEntity, IEnumerable<TOther>>> prop,
        Expression<Func<TEntity, IDataContext, IQueryable<TOther>>> queryExpression,
        bool? canBeNull = null
    ) => _entityBuilder.Association(prop, queryExpression, canBeNull);

    /// <summary>
    /// Adds one-to-one association mapping to current entity.
    /// </summary>
    /// <typeparam name="TOther">Other association side type</typeparam>
    /// <param name="prop">Association member getter expression.</param>
    /// <param name="queryExpression">Query expression.</param>
    /// <param name="canBeNull">Defines type of join. True - left join, False - inner join.</param>
    /// <returns>Returns fluent property mapping builder.</returns>
    public void Association<TOther>(
        Expression<Func<TEntity, TOther>> prop,
        Expression<Func<TEntity, IDataContext, IQueryable<TOther>>> queryExpression,
        bool? canBeNull = null
    ) => _entityBuilder.Association(prop, queryExpression, canBeNull);

    /// <summary>Adds primary key mapping to current entity.</summary>
    /// <param name="func">Primary key getter expression.</param>
    /// <param name="order">Primary key field order.
    /// When multiple fields specified by getter expression, fields will be ordered from first mentioned
    /// field to last one starting from provided order with step <c>1</c>.</param>
    /// <returns>Returns current fluent entity mapping builder.</returns>
    public void HasPrimaryKey<TProperty>(
        Expression<Func<TEntity, TProperty>> func,
        int order = -1
    ) => _entityBuilder.HasPrimaryKey(func, order);

    /// <summary>Adds identity column mapping to current entity.</summary>
    /// <param name="func">Identity field getter expression.</param>
    /// <returns>Returns current fluent entity mapping builder.</returns>
    public void HasIdentity<TProperty>(Expression<Func<TEntity, TProperty>> func) =>
        _entityBuilder.HasIdentity(func);

    /// <summary>Adds column mapping to current entity.</summary>
    /// <param name="func">Column member getter expression.</param>
    /// <param name="order">Unused.</param>
    /// <returns>Returns current fluent entity mapping builder.</returns>
    public void HasColumn(Expression<Func<TEntity, object?>> func, int order = -1) =>
        _entityBuilder.HasColumn(func, order);

    /// <summary>
    /// Instruct LINQ to DB to not incude specified member into mapping.
    /// </summary>
    /// <param name="func">Member getter expression.</param>
    /// <param name="order">Unused.</param>
    /// <returns>Returns current fluent entity mapping builder.</returns>
    public void Ignore(Expression<Func<TEntity, object?>> func, int order = -1) =>
        _entityBuilder.Ignore(func, order);

    /// <summary>
    ///     Specifies a LINQ predicate expression that will automatically be applied to any queries targeting
    ///     this entity type.
    /// </summary>
    /// <param name="filter"> The LINQ predicate expression. </param>
    /// <returns> The same builder instance so that multiple configuration calls can be chained. </returns>
    public void HasQueryFilter(Expression<Func<TEntity, IDataContext, bool>> filter) =>
        _entityBuilder.HasQueryFilter(filter);

    /// <summary>
    ///     Specifies a LINQ predicate expression that will automatically be applied to any queries targeting
    ///     this entity type.
    /// </summary>
    /// <param name="filter"> The LINQ predicate expression. </param>
    /// <returns> The same builder instance so that multiple configuration calls can be chained. </returns>
    public void HasQueryFilter(Expression<Func<TEntity, bool>> filter) =>
        _entityBuilder.HasQueryFilter(filter);

    /// <summary>
    ///     Specifies a LINQ predicate expression that will automatically be applied to any queries targeting
    ///     this entity type.
    /// </summary>
    /// <param name="filter"> The LINQ predicate expression. </param>
    /// <returns> The same builder instance so that multiple configuration calls can be chained. </returns>
    public void HasQueryFilter<TDataContext>(Expression<Func<TEntity, TDataContext, bool>> filter)
        where TDataContext : IDataContext => _entityBuilder.HasQueryFilter(filter);

    /// <summary>
    ///     Specifies a LINQ <see cref="T:System.Linq.IQueryable`1" /> function that will automatically be applied to any queries targeting
    ///     this entity type.
    /// </summary>
    /// <param name="filterFunc">Function which corrects input IQueryable.</param>
    /// <returns> The same builder instance so that multiple configuration calls can be chained. </returns>
    public void HasQueryFilter(
        Func<IQueryable<TEntity>, IDataContext, IQueryable<TEntity>> filterFunc
    ) => HasQueryFilter<IDataContext>(filterFunc);

    /// <summary>
    ///     Specifies a LINQ <see cref="T:System.Linq.IQueryable`1" /> function that will automatically be applied to any queries targeting
    ///     this entity type.
    /// </summary>
    /// <param name="filterFunc"> The LINQ predicate expression. </param>
    /// <returns> The same builder instance so that multiple configuration calls can be chained. </returns>
    public void HasQueryFilter<TDataContext>(
        Func<IQueryable<TEntity>, TDataContext, IQueryable<TEntity>> filterFunc
    )
        where TDataContext : IDataContext => _entityBuilder.HasQueryFilter(filterFunc);

    internal MappingSchema Build()
    {
        _entityBuilder.HasTableName(TableName);
        _builder.Build();
        return _builder.MappingSchema;
    }
}
