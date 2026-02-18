using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IGenericRepository<T> where T  :BaseEntity
    {
        Task<T> GetByIdAsync(int id, bool ignoreQueryFilters = false, params Expression<Func<T, object>>[] includes);

        Task<IReadOnlyList<T>> ListAllAsync(bool ignoreQueryFilters = false, params Expression<Func<T, object>>[] includes);

        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
