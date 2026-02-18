using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class ECommerceDbContext:DbContext
    {
        public ECommerceDbContext(DbContextOptions<ECommerceDbContext> options) : base(options)
        {
        }
        public DbSet<Core.Entities.Product> Products { get; set; }
        public DbSet<Core.Entities.Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.ApplyConfigurationsFromAssembly(System.Reflection.Assembly.GetExecutingAssembly());
            //Global Query Filter: Silinen ürünler ve kategoriler varsayılan olarak sorgulardan hariç tutulur.
            modelBuilder.Entity<Core.Entities.Product>().HasQueryFilter(p => !p.IsDeleted);
            modelBuilder.Entity<Core.Entities.Category>().HasQueryFilter(c => !c.IsDeleted);
        }

    }
}
