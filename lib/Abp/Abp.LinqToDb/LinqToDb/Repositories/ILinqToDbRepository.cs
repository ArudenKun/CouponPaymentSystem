using System.Linq.Expressions;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using LinqToDB.Linq;

namespace Abp.LinqToDb.Repositories;

public interface ILinqToDbRepository<TEntity> : IRepository
    where TEntity : class, IEntity<int> { }

public interface ILinqToDbRepository<TEntity, TPrimaryKey> : IRepository
    where TEntity : class, IEntity<TPrimaryKey>
{
    /// <summary>
    /// Used to run a query over entire entities.
    /// </summary>
    /// <typeparam name="TEntity">Type of return value of this method</typeparam>
    /// <returns>Query result</returns>
    IQueryable<TEntity> Query();

    /// <summary>Gets an entity with given primary key.</summary>
    /// <param name="id">Primary key of the entity to get</param>
    /// <returns>Entity</returns>
    TEntity Get(TPrimaryKey id);

    /// <summary>Gets an entity with given primary key.</summary>
    /// <param name="id">Primary key of the entity to get</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>Entity</returns>
    Task<TEntity> GetAsync(TPrimaryKey id, CancellationToken cancellationToken);

    /// <summary>
    /// Gets exactly one entity with given primary key.
    /// Throws exception if no entity or more than one entity.
    /// </summary>
    TEntity Single(TPrimaryKey id);

    /// <summary>
    /// Gets exactly one entity with given predicate.
    /// Throws exception if no entity or more than one entity.
    /// </summary>
    /// <param name="predicate">Entity</param>
    TEntity Single(Func<TEntity, bool> predicate);

    /// <summary>
    /// Gets exactly one entity with given predicate.
    /// Throws exception if no entity or more than one entity.
    /// </summary>
    /// <param name="predicate">Entity</param>
    TEntity Single(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// Gets exactly one entity with given primary key.
    /// Throws exception if no entity or more than one entity.
    /// </summary>
    Task<TEntity> SingleAsync(TPrimaryKey id, CancellationToken cancellationToken);

    /// <summary>
    /// Gets exactly one entity with given predicate.
    /// Throws exception if no entity or more than one entity.
    /// </summary>
    /// <param name="predicate">Entity</param>
    /// <param name="cancellationToken">CancellationToken</param>
    Task<TEntity> SingleAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken
    );

    /// <summary>
    /// Gets an entity with given primary key or null if not found.
    /// </summary>
    /// <param name="id">Primary key of the entity to get</param>
    /// <returns>Entity or null</returns>
    TEntity? FirstOrDefault(TPrimaryKey id);

    /// <summary>
    /// Gets an entity with given predicate or null if not found.
    /// </summary>
    /// <param name="predicate">Predicate to filter entities</param>
    TEntity? FirstOrDefault(Func<TEntity, bool> predicate);

    /// <summary>
    /// Gets an entity with given predicate or null if not found.
    /// </summary>
    /// <param name="predicate">Predicate to filter entities</param>
    TEntity? FirstOrDefault(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// Gets an entity with given primary key or null if not found.
    /// </summary>
    /// <param name="id">Primary key of the entity to get</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>Entity or null</returns>
    Task<TEntity?> FirstOrDefaultAsync(TPrimaryKey id, CancellationToken cancellationToken);

    /// <summary>
    /// Gets an entity with given predicate or null if not found.
    /// </summary>
    /// <param name="predicate">Predicate to filter entities</param>
    /// <param name="cancellationToken">CancellationToken</param>
    Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken
    );

    /// <summary>Inserts a new entity.</summary>
    /// <param name="entity">Inserted entity</param>
    TEntity Insert(TEntity entity);

    /// <summary>Inserts a new entity.</summary>
    /// <param name="entity">Inserted entity</param>
    /// <param name="cancellationToken">CancellationToken</param>
    Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken);

    /// <summary>
    /// Inserts a new entity and gets it's Id.
    /// It may require to save current unit of work
    /// to be able to retrieve id.
    /// </summary>
    /// <param name="entity">Entity</param>
    /// <returns>Id of the entity</returns>
    TPrimaryKey InsertAndGetId(TEntity entity);

    /// <summary>
    /// Inserts a new entity and gets it's Id.
    /// It may require to save current unit of work
    /// to be able to retrieve id.
    /// </summary>
    /// <param name="entity">Entity</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>Id of the entity</returns>
    Task<TPrimaryKey> InsertAndGetIdAsync(TEntity entity, CancellationToken cancellationToken);

    /// <summary>
    /// Inserts or updates given entity depending on Id's value.
    /// </summary>
    /// <param name="entity">Entity</param>
    TEntity InsertOrUpdate(TEntity entity);

    /// <summary>
    /// Inserts or updates given entity depending on Id's value.
    /// </summary>
    /// <param name="entity">Entity</param>
    /// <param name="cancellationToken">CancellationToken</param>
    Task<TEntity> InsertOrUpdateAsync(TEntity entity, CancellationToken cancellationToken);

    /// <summary>
    /// Inserts or updates given entity depending on Id's value.
    /// Also returns Id of the entity.
    /// It may require to save current unit of work
    /// to be able to retrieve id.
    /// </summary>
    /// <param name="entity">Entity</param>
    /// <returns>Id of the entity</returns>
    TPrimaryKey InsertOrUpdateAndGetId(TEntity entity);

    /// <summary>
    /// Inserts or updates given entity depending on Id's value.
    /// Also returns Id of the entity.
    /// It may require to save current unit of work
    /// to be able to retrieve id.
    /// </summary>
    /// <param name="entity">Entity</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>Id of the entity</returns>
    Task<TPrimaryKey> InsertOrUpdateAndGetIdAsync(
        TEntity entity,
        CancellationToken cancellationToken
    );

    /// <summary>Updates an existing entity.</summary>
    /// <param name="entity">Entity</param>
    TEntity Update(TEntity entity);

    /// <summary>Updates an existing entity.</summary>
    /// <param name="entity">Entity</param>
    /// <param name="cancellationToken">CancellationToken</param>
    Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken);

    /// <summary>Deletes an entity.</summary>
    /// <param name="entity">Entity to be deleted</param>
    void Delete(TEntity entity);

    /// <summary>Deletes an entity by primary key.</summary>
    /// <param name="id">Primary key of the entity</param>
    void Delete(TPrimaryKey id);

    /// <summary>
    /// Deletes many entities by function.
    /// Notice that: All entities fits to given predicate are retrieved and deleted.
    /// This may cause major performance problems if there are too many entities with
    /// given predicate.
    /// </summary>
    /// <param name="predicate">A condition to filter entities</param>
    void Delete(Expression<Func<TEntity, bool>> predicate);

    /// <summary>Deletes an entity.</summary>
    /// <param name="entity">Entity to be deleted</param>
    /// <param name="cancellationToken">CancellationToken</param>
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken);

    /// <summary>Deletes an entity by primary key.</summary>
    /// <param name="id">Primary key of the entity</param>
    /// <param name="cancellationToken">CancellationToken</param>
    Task DeleteAsync(TPrimaryKey id, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes many entities by function.
    /// Notice that: All entities fits to given predicate are retrieved and deleted.
    /// This may cause major performance problems if there are too many entities with
    /// given predicate.
    /// </summary>
    /// <param name="predicate">A condition to filter entities</param>
    /// <param name="cancellationToken"></param>
    Task DeleteAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken
    );

    /// <summary>Gets count of all entities in this repository.</summary>
    /// <returns>Count of entities</returns>
    int Count();

    /// <summary>
    /// Gets count of all entities in this repository based on given <paramref name="predicate" />.
    /// </summary>
    /// <param name="predicate">A method to filter count</param>
    /// <returns>Count of entities</returns>
    int Count(Func<TEntity, bool> predicate);

    /// <summary>
    /// Gets count of all entities in this repository based on given <paramref name="predicate" />.
    /// </summary>
    /// <param name="predicate">A method to filter count</param>
    /// <returns>Count of entities</returns>
    int Count(Expression<Func<TEntity, bool>> predicate);

    /// <summary>Gets count of all entities in this repository.</summary>
    /// <returns>Count of entities</returns>
    Task<int> CountAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Gets count of all entities in this repository based on given <paramref name="predicate" />.
    /// </summary>
    /// <param name="predicate">A method to filter count</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Count of entities</returns>
    Task<int> CountAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken
    );

    /// <summary>
    /// Gets count of all entities in this repository (use if expected return value is greater than <see cref="F:System.Int32.MaxValue" />.
    /// </summary>
    /// <returns>Count of entities</returns>
    long LongCount();

    /// <summary>
    /// Gets count of all entities in this repository based on given <paramref name="predicate" />
    /// (use this overload if expected return value is greater than <see cref="F:System.Int32.MaxValue" />).
    /// </summary>
    /// <param name="predicate">A method to filter count</param>
    /// <returns>Count of entities</returns>
    long LongCount(Func<TEntity, bool> predicate);

    /// <summary>
    /// Gets count of all entities in this repository based on given <paramref name="predicate" />
    /// (use this overload if expected return value is greater than <see cref="F:System.Int32.MaxValue" />).
    /// </summary>
    /// <param name="predicate">A method to filter count</param>
    /// <returns>Count of entities</returns>
    long LongCount(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// Gets count of all entities in this repository (use if expected return value is greater than <see cref="F:System.Int32.MaxValue" />.
    /// </summary>
    /// <returns>Count of entities</returns>
    Task<long> LongCountAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Gets count of all entities in this repository based on given <paramref name="predicate" />
    /// (use this overload if expected return value is greater than <see cref="F:System.Int32.MaxValue" />).
    /// </summary>
    /// <param name="predicate">A method to filter count</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Count of entities</returns>
    Task<long> LongCountAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken
    );
}
