using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        // Tekil Getir (Silineni görme opsiyonu var)
        Task<T> GetByIdAsync(int id, bool ignoreQueryFilters = false, params Expression<Func<T, object>>[] includes);

        // Çoğul Getir (Sayfalama Zorunlu)
        // DİKKAT: Buradan 'bool' parametresini kaldırdım. Sadece Skip, Take ve Include var.
        Task<IReadOnlyList<T>> ListAllAsync(int skip, int take, params Expression<Func<T, object>>[] includes);

        Task<int> CountAsync();
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}