using Core.Entities;
using Core.Interfaces;
using Infrastructure.Persistence;
using System.Collections;


namespace Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ECommerceDbContext _context;
        private Hashtable _repositories;

        public UnitOfWork(ECommerceDbContext context)
        {
            _context = context;
        }

        public async Task<int> Complete()
        {
            // İşte o beklenen an: Tüm değişiklikler burada tek seferde yansıtılır.
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            if (_repositories == null) _repositories = new Hashtable();

            var type = typeof(TEntity).Name;

            // Eğer bu repository daha önce oluşturulmadıysa oluştur ve listeye ekle.
            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(GenericRepository<>);

                // GenericRepository<T>'yi, TEntity tipiyle instance alıyoruz.
                var repositoryInstance = Activator.CreateInstance(
                    repositoryType.MakeGenericType(typeof(TEntity)),
                    _context);

                _repositories.Add(type, repositoryInstance);
            }

            return (IGenericRepository<TEntity>)_repositories[type];
        }
    }
}