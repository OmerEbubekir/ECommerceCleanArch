using Core.Entities;
using System;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        // İstediğimiz Entity'nin repository'sini çağırmamızı sağlar.
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;

        // Tüm işlemleri veritabanına commit eder (SaveChanges).
        Task<int> Complete();
    }
}