using Ecom.Core.Interfaces;
using System.Linq.Expressions;

namespace Ecom.Infrastructure.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    Task<T> IGenericRepository<T>.AddAsync(T entity)
    {
        throw new NotImplementedException();
    }

    Task IGenericRepository<T>.DeleteAsync(T entity)
    {
        throw new NotImplementedException();
    }

    Task<IReadOnlyList<T>> IGenericRepository<T>.GetAllAsync()
    {
        throw new NotImplementedException();
    }

    Task<IReadOnlyList<T>> IGenericRepository<T>.GetAllAsync(params Expression<Func<T, object>>[] includes)
    {
        throw new NotImplementedException();
    }

    Task<T> IGenericRepository<T>.GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    Task<T> IGenericRepository<T>.GetByIdAsync(int id, params Expression<Func<T, object>>[] includes)
    {
        throw new NotImplementedException();
    }

    Task IGenericRepository<T>.UpdateAsync(T entity)
    {
        throw new NotImplementedException();
    }
}
