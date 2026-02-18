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

        public Task DeleteAsync(T entity)
        {
           _context.Set<T>().Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes)
        {
            // 1. Sorguyu IQueryable olarak başlat (Henüz DB'ye gitmedi)
            IQueryable<T> query = _context.Set<T>();

            // 2. Include'ları ekle (Varsa)
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            // 3. Veriyi ID'ye göre filtrele ve getir.
            // BaseEntity içindeki "ID" property'sini kullanıyoruz.
            return await query.FirstOrDefaultAsync(x => x.ID == id);
        }

        public async Task<IReadOnlyList<T>> ListAllAsync(params Expression<Func<T, object>>[] includes)
        {
            // Query'yi başlatıyoruz ama hemen veritabanına gitmiyoruz (IQueryable)
            IQueryable<T> query = _context.Set<T>().AsNoTracking();

            // Eğer parametre olarak "Category'yi de getir" dendiyse, query'ye ekliyoruz.
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            // ToListAsync dediğimiz an sorgu çalışır ve veriler dolu gelir.
            return await query.ToListAsync();
        }

        public Task UpdateAsync(T entity)
        {
           _context.Entry(entity).State = EntityState.Modified;
            return Task.CompletedTask;
        }
    }
}
