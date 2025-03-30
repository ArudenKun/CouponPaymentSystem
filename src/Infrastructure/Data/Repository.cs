using System.Data;
using System.Linq.Expressions;
using Application.Common.Interfaces.Data;
using Domain.Common.Entities;
using Dommel;

namespace Infrastructure.Data;

internal class Repository<TEntity> : IRepository<TEntity>
    where TEntity : class, IEntity
{
    public Repository(IDbConnection connection)
    {
        Connection = connection;
    }

    protected IDbConnection Connection { get; }

    public TEntity? GetById(int id)
    {
        return Connection.Get<TEntity>(id);
    }

    public TEntity? GetById<T2>(int id)
    {
        return Connection.Get<TEntity, T2, TEntity>(id);
    }

    public TEntity? GetById<T2, T3>(int id)
    {
        return Connection.Get<TEntity, T2, T3, TEntity>(id);
    }

    public TEntity? GetById<T2, T3, T4>(int id)
    {
        return Connection.Get<TEntity, T2, T3, T4, TEntity>(id);
    }

    public TEntity? GetById<T2, T3, T4, T5>(int id)
    {
        return Connection.Get<TEntity, T2, T3, T4, T5, TEntity>(id);
    }

    public TEntity? GetById<T2, T3, T4, T5, T6>(int id)
    {
        return Connection.Get<TEntity, T2, T3, T4, T5, T6, TEntity>(id);
    }

    public IEnumerable<TEntity> GetAll()
    {
        return Connection.GetAll<TEntity>();
    }

    public IEnumerable<TEntity> GetAll<T2>()
    {
        return Connection.GetAll<TEntity, T2, TEntity>();
    }

    public IEnumerable<TEntity> GetAll<T2, T3>()
    {
        return Connection.GetAll<TEntity, T2, T3, TEntity>();
    }

    public IEnumerable<TEntity> GetAll<T2, T3, T4>()
    {
        return Connection.GetAll<TEntity, T2, T3, T4, TEntity>();
    }

    public IEnumerable<TEntity> GetAll<T2, T3, T4, T5>()
    {
        return Connection.GetAll<TEntity, T2, T3, T4, T5, TEntity>();
    }

    public IEnumerable<TEntity> GetAll<T2, T3, T4, T5, T6>()
    {
        return Connection.GetAll<TEntity, T2, T3, T4, T5, T6, TEntity>();
    }

    public object Insert(TEntity entity)
    {
        return Connection.Insert(entity);
    }

    public bool Delete(TEntity entity)
    {
        return Connection.Delete(entity);
    }

    public int DeleteMultiple(Expression<Func<TEntity, bool>> predicate)
    {
        return Connection.DeleteMultiple(predicate);
    }

    public int DeleteAll()
    {
        return Connection.DeleteAll<TEntity>();
    }

    public bool Update(TEntity entity)
    {
        return Connection.Update(entity);
    }

    public IEnumerable<TEntity> Select(Expression<Func<TEntity, bool>> predicate)
    {
        return Connection.Select(predicate);
    }

    public IEnumerable<TEntity> SelectPaged(
        Expression<Func<TEntity, bool>> predicate,
        int pageNumber,
        int pageSize
    )
    {
        return Connection.SelectPaged(predicate, pageNumber, pageSize);
    }

    public TEntity? FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
    {
        return Connection.FirstOrDefault(predicate);
    }

    public Task<TEntity?> GetByIdAsync(int id)
    {
        return Connection.GetAsync<TEntity>(id);
    }

    public Task<TEntity?> GetByIdAsync<T2>(int id)
    {
        return Connection.GetAsync<TEntity, T2, TEntity>(id);
    }

    public Task<TEntity?> GetByIdAsync<T2, T3>(int id)
    {
        return Connection.GetAsync<TEntity, T2, T3, TEntity>(id);
    }

    public Task<TEntity?> GetByIdAsync<T2, T3, T4>(int id)
    {
        return Connection.GetAsync<TEntity, T2, T3, T4, TEntity>(id);
    }

    public Task<TEntity?> GetByIdAsync<T2, T3, T4, T5>(int id)
    {
        return Connection.GetAsync<TEntity, T2, T3, T4, T5, TEntity>(id);
    }

    public Task<TEntity?> GetByIdAsync<T2, T3, T4, T5, T6>(int id)
    {
        return Connection.GetAsync<TEntity, T2, T3, T4, T5, T6, TEntity>(id);
    }

    public Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return Connection.GetAllAsync<TEntity>();
    }

    public Task<IEnumerable<TEntity>> GetAllAsync<T2>()
    {
        return Connection.GetAllAsync<TEntity, T2, TEntity>();
    }

    public Task<IEnumerable<TEntity>> GetAllAsync<T2, T3>()
    {
        return Connection.GetAllAsync<TEntity, T2, T3, TEntity>();
    }

    public Task<IEnumerable<TEntity>> GetAllAsync<T2, T3, T4>()
    {
        return Connection.GetAllAsync<TEntity, T2, T3, T4, TEntity>();
    }

    public Task<IEnumerable<TEntity>> GetAllAsync<T2, T3, T4, T5>()
    {
        return Connection.GetAllAsync<TEntity, T2, T3, T4, T5, TEntity>();
    }

    public Task<IEnumerable<TEntity>> GetAllAsync<T2, T3, T4, T5, T6>()
    {
        return Connection.GetAllAsync<TEntity, T2, T3, T4, T5, T6, TEntity>();
    }

    public Task<object> InsertAsync(TEntity entity)
    {
        return Connection.InsertAsync(entity);
    }

    public Task<bool> DeleteAsync(TEntity entity)
    {
        return Connection.DeleteAsync(entity);
    }

    public Task<int> DeleteAllAsync()
    {
        return Connection.DeleteAllAsync<TEntity>();
    }

    public Task<int> DeleteMultipleAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return Connection.DeleteMultipleAsync(predicate);
    }

    public Task<bool> UpdateAsync(TEntity entity)
    {
        return Connection.UpdateAsync(entity);
    }

    public Task<IEnumerable<TEntity>> SelectAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return Connection.SelectAsync(predicate);
    }

    public Task<IEnumerable<TEntity>> SelectPagedAsync(
        Expression<Func<TEntity, bool>> predicate,
        int pageNumber,
        int pageSize
    )
    {
        return Connection.SelectPagedAsync(predicate, pageNumber, pageSize);
    }

    public Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return Connection.FirstOrDefaultAsync(predicate);
    }
}
