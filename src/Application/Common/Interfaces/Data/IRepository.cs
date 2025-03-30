using System.Linq.Expressions;
using Domain.Common.Entities;

namespace Application.Common.Interfaces.Data;

public interface IRepository<TEntity>
    where TEntity : class, IEntity
{
    TEntity? GetById(int id);
    TEntity? GetById<T2>(int id);
    TEntity? GetById<T2, T3>(int id);
    TEntity? GetById<T2, T3, T4>(int id);
    TEntity? GetById<T2, T3, T4, T5>(int id);
    TEntity? GetById<T2, T3, T4, T5, T6>(int id);

    IEnumerable<TEntity> GetAll();
    IEnumerable<TEntity> GetAll<T2>();
    IEnumerable<TEntity> GetAll<T2, T3>();
    IEnumerable<TEntity> GetAll<T2, T3, T4>();
    IEnumerable<TEntity> GetAll<T2, T3, T4, T5>();
    IEnumerable<TEntity> GetAll<T2, T3, T4, T5, T6>();

    object Insert(TEntity entity);

    bool Delete(TEntity entity);
    int DeleteMultiple(Expression<Func<TEntity, bool>> predicate);
    int DeleteAll();

    bool Update(TEntity entity);

    IEnumerable<TEntity> Select(Expression<Func<TEntity, bool>> predicate);
    IEnumerable<TEntity> SelectPaged(
        Expression<Func<TEntity, bool>> predicate,
        int pageNumber,
        int pageSize
    );

    TEntity? FirstOrDefault(Expression<Func<TEntity, bool>> predicate);

    Task<TEntity?> GetByIdAsync(int id);
    Task<TEntity?> GetByIdAsync<T2>(int id);
    Task<TEntity?> GetByIdAsync<T2, T3>(int id);
    Task<TEntity?> GetByIdAsync<T2, T3, T4>(int id);
    Task<TEntity?> GetByIdAsync<T2, T3, T4, T5>(int id);
    Task<TEntity?> GetByIdAsync<T2, T3, T4, T5, T6>(int id);

    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<IEnumerable<TEntity>> GetAllAsync<T2>();
    Task<IEnumerable<TEntity>> GetAllAsync<T2, T3>();
    Task<IEnumerable<TEntity>> GetAllAsync<T2, T3, T4>();
    Task<IEnumerable<TEntity>> GetAllAsync<T2, T3, T4, T5>();
    Task<IEnumerable<TEntity>> GetAllAsync<T2, T3, T4, T5, T6>();

    Task<object> InsertAsync(TEntity entity);

    Task<bool> DeleteAsync(TEntity entity);
    Task<int> DeleteMultipleAsync(Expression<Func<TEntity, bool>> predicate);
    Task<int> DeleteAllAsync();

    Task<bool> UpdateAsync(TEntity entity);

    Task<IEnumerable<TEntity>> SelectAsync(Expression<Func<TEntity, bool>> predicate);
    Task<IEnumerable<TEntity>> SelectPagedAsync(
        Expression<Func<TEntity, bool>> predicate,
        int pageNumber,
        int pageSize
    );

    Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
}
