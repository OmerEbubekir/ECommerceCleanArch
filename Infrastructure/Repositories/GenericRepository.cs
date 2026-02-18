using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly ECommerceDbContext _context;
        public GenericRepository(ECommerceDbContext context)
        {
            _context = context;
        }
        public async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            return entity;  
        }

        public async Task DeleteAsync(T entity)
        {
            // ESKİSİ: _context.Set<T>().Remove(entity);

            // YENİSİ (Soft Delete):
            entity.IsDeleted = true;
            _context.Set<T>().Update(entity); // Remove değil Update!

            // Asenkron arayüze uymak için
            await Task.CompletedTask;
        }

        public async Task<T> GetByIdAsync(int id, bool ignoreQueryFilters = false, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();

            // Eğer "Filtreyi Boşver" denildiyse, EF Core'a bunu bildir.
            if (ignoreQueryFilters)
            {
                query = query.IgnoreQueryFilters();
            }

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            // SingleOrDefault yerine FirstOrDefault daha güvenlidir.
            return await query.FirstOrDefaultAsync(x => x.ID == id);
        }

        public async Task<IReadOnlyList<T>> ListAllAsync(bool ignoreQueryFilters = false, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>().AsNoTracking();

            // Global query filter'ları devre dışı bırakmak isteniyorsa, EF Core'a bunu bildir.
            if (ignoreQueryFilters)
            {
                query = query.IgnoreQueryFilters();
            }

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return await query.ToListAsync();
        }
        public Task UpdateAsync(T entity)
        {
           _context.Entry(entity).State = EntityState.Modified;
            return Task.CompletedTask;
        }
    }
}
